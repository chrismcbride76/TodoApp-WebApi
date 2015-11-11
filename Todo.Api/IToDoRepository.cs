using System.Collections.Generic;
using Todo.Api.Models;

namespace Todo.Api
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