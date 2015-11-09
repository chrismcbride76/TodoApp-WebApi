using System;

namespace Todo.Api.Models
{
    public class Todo
    {
        public String Task { get; set; }
        public DateTime Deadline { get; set; }

        public bool IsCompleted { get; set; }
    }
}