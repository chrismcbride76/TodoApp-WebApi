using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Todo.Api.Models;

namespace Todo.Api
{
    public class InMemoryTodoRepository : IToDoRepository
    {
        private readonly List<TodoModel> _data;
        private int _lastId = 0;

        public InMemoryTodoRepository(List<TodoModel> initialDataSet = null)
        {
            _data = initialDataSet ?? new List<TodoModel>();
        }

        public IEnumerable<TodoModel> GetAll()
        {
            return _data.AsReadOnly();
        }

        public TodoModel Get(int id)
        {
            return _data.FirstOrDefault(x => x.Id == id);
        }

        public TodoModel Add(TodoModel item)
        {
            int id = Interlocked.Increment(ref _lastId);
            item.Id = id;

            _data.Add(item);

            return item;
        }

        public void Remove(int id)
        {
            _data.RemoveAll(x => x.Id == id);
        }

        public bool Update(TodoModel item)
        {
            var entry = _data.FirstOrDefault(x => x.Id == item.Id);
            if (entry == null)
                return false;

            entry = item;
            return true;
        }
    }
}