using System.Collections.Generic;
using Todo.Api.Models;

namespace Todo.Api
{
    public interface IToDoRepository
    {
        IEnumerable<TodoModel> GetAll();
        TodoModel Get(int id);

        TodoModel Add(TodoModel item);

        void Remove(int id);

        bool Update(TodoModel item);
    }
}