using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoAPI.Dtos
{
    public class PaginatedResultDto<T>
    {
        public List<T> Results { get; set; }
        public int TotalRecordCount { get; set; }
    }
}