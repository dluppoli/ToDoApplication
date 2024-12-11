using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoMVC.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Results { get; set; }
        public int TotalRecordCount { get; set; }
    }
}