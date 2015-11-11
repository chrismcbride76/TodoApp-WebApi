using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Models
{
    public class ToDo: Resource
    {
        public int id { get; set; }

        [Required]
        public String task { get; set; }

        [Required]
        public DateTime deadlineUtc { get; set; }

        [Required]
        public bool isCompleted { get; set; }

        public String moreDetails { get; set; }
    }
}