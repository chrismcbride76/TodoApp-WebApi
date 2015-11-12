using System;

namespace Todo.Api.Models
{
    public class TodoModel
    {
        public int id { get; set; }

        public String task { get; set; }

        public DateTime deadlineUtc { get; set; }

        public bool completed { get; set; }

        public String moreDetails { get; set; }
    }
}