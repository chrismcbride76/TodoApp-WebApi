using System;

namespace Todo.Api.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        public String Task { get; set; }
        public DateTime DeadlineUtc { get; set; }

        public bool IsCompleted { get; set; }
    }
}