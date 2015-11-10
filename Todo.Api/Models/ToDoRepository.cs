using System.Collections.Generic;

namespace Todo.Api.Models
{
    public class ToDoRepository : IToDoRepository
    {
        public IEnumerable<ToDo> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public ToDo Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public ToDo Add(ToDo item)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(ToDo item)
        {
            throw new System.NotImplementedException();
        }
    }
}