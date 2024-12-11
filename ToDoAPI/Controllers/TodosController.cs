using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ToDoAPI.Dtos;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers
{
    [RoutePrefix("api/todos")]
    public class TodosController : ApiController
    {
        private ToDoAppEntities context;
        private MapperConfiguration mc;
        private Mapper mapper;

        public TodosController()
        {
            context = new ToDoAppEntities();
            mc = new MapperConfiguration(c => c.AddProfile<DtoMappingProfile>());
            mapper = new Mapper(mc);
        }

        public async Task<IHttpActionResult> Get(int page=1, int pagesize=10, bool excludeCompleted=true,string sortBy="id",string sortOrder="asc")
        {
            var query = context.Todos
                .Where(q => excludeCompleted == false || q.Completed == false)
                .ProjectTo<TodoDto>(mc);

            int totalRecordCount = await query.CountAsync();

            switch(sortBy)
            {
                case "text":
                    if(sortOrder=="desc")
                        query = query.OrderByDescending(q => q.Text); 
                    else
                        query = query.OrderBy(q => q.Text);
                    break;
                case "expire":
                    if (sortOrder == "desc")
                        query = query.OrderByDescending(q => q.Expire);
                    else
                        query = query.OrderBy(q => q.Expire);
                    break;
                default:
                    if (sortOrder == "desc")
                        query = query.OrderByDescending(q => q.Id);
                    else
                        query = query.OrderBy(q => q.Id);
                    break;
            }

            var retval = await query.Skip((page - 1) * pagesize).Take(pagesize).ToListAsync();


            return Ok(new PaginatedResultDto<TodoDto>() { 
                Results = retval,
                TotalRecordCount = totalRecordCount
            });
        }

        public async Task<IHttpActionResult> Post([FromBody] TodoDto nuovDto)
        {
            try
            {
                System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(nuovDto);
                List<ValidationResult> validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(nuovDto, validationContext, validationResults, true);
                if (!isValid) return BadRequest();

                var nuovo = mapper.Map<Todo>(nuovDto);
                context.Todos.Add(nuovo);
                await context.SaveChangesAsync();
                return Ok(mapper.Map<TodoDto>(nuovo));
            }
            catch
            {
                return BadRequest();
            }
        }

        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0) return BadRequest();

                var candidate = await context.Todos.FirstOrDefaultAsync(q => q.Id == id);
                if (candidate == null) return NotFound();

                context.Todos.Remove(candidate);
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("{id}/complete")]
        public async Task<IHttpActionResult> Complete(int id) 
        {
            return await SetComplete(id, true);
        }

        [HttpPost]
        [Route("{id}/uncomplete")]
        public async Task<IHttpActionResult> UnComplete(int id)
        {
            return await SetComplete(id, false);
        }

        private async Task<IHttpActionResult> SetComplete(int id, bool newValue)
        {
            try
            {
                if (id <= 0) return BadRequest();

                var candidate = await context.Todos.FirstOrDefaultAsync(q => q.Id == id);
                if (candidate == null) return NotFound();

                candidate.Completed = true;
                await context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}