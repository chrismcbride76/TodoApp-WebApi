using System;

namespace Todo.Api.Models
{
    public class TodoModel
    {
        public int Id { get; set; }

        public String Task { get; set; }

        public DateTime DeadlineUtc { get; set; }

        public bool Completed { get; set; }

        public String MoreDetails { get; set; }
    }
}