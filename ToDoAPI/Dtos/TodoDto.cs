using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoAPI.Dtos
{
    public class TodoDto
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        public string Text { get; set; }
        public DateTime? Expire { get; set; }
        public bool Completed { get; set; }
    }
}