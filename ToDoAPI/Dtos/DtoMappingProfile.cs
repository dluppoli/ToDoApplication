using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoAPI.Models;

namespace ToDoAPI.Dtos
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<TodoDto, Todo>();
            CreateMap<Todo, TodoDto>();
        }
    }
}