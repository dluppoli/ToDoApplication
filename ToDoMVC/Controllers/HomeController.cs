using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ToDoMVC.Models;

namespace ToDoMVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(int page= 1)
        {
            string baseUrl = ConfigurationManager.AppSettings["BaseApiUrl"];
            int pageSize = 10;
            var client = new RestClient(baseUrl);
            var request = new RestRequest();
            request.AddQueryParameter("page", page.ToString());
            request.AddQueryParameter("pagesize", pageSize.ToString());
            var response = await client.GetAsync<PaginatedResult<Todo>>(request);

            return View(response);
        }

        [HttpPost]
        public async Task<ActionResult> Index(Todo nuovo)
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}