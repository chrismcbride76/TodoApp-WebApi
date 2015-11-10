using System.Collections.Generic;

namespace Todo.Api.Models
{
    public interface IToDoRepository
    {
        IEnumerable<ToDo> GetAll();
        ToDo Get(int id);

        ToDo Add(ToDo item);

        void Remove(int id);

        bool Update(ToDo item);
    }
}