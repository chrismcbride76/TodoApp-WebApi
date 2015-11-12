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
            return _data.FirstOrDefault(x => x.id == id);
        }

        public TodoModel Add(TodoModel item)
        {
            int id = Interlocked.Increment(ref _lastId);
            item.id = id;

            _data.Add(item);

            return item;
        }

        public bool Delete(int id)
        {
            return _data.RemoveAll(x => x.id == id) > 0;
        }

        public bool Update(TodoModel item)
        {
            var entry = _data.FirstOrDefault(x => x.id == item.id);
            if (entry == null)
                return false;

            entry.completed = item.completed;
            entry.deadlineUtc = item.deadlineUtc;
            entry.moreDetails = item.moreDetails;
            entry.task = item.task;
            return true;
        }
    }
}