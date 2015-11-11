using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Models
{
    public class ToDo: Resource
    {
        public int Id { get; set; }

        [Required]
        public String Task { get; set; }

        [Required]
        public DateTime DeadlineUtc { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        public String MoreDetails { get; set; }
    }
}