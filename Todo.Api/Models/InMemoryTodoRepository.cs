using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Todo.Api.Models
{
    public class InMemoryTodoRepository : IToDoRepository
    {
        private readonly List<ToDo> _data;
        private int _lastId = 0;

        public InMemoryTodoRepository(List<ToDo> initialDataSet = null)
        {
            _data = initialDataSet ?? new List<ToDo>();
        }

        public IEnumerable<ToDo> GetAll()
        {
            return _data.AsReadOnly();
        }

        public ToDo Get(int id)
        {
            return _data.FirstOrDefault(x => x.Id == id);
        }

        public ToDo Add(ToDo item)
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

        public bool Update(ToDo item)
        {
            var entry = _data.FirstOrDefault(x => x.Id == item.Id);
            if (entry == null)
                return false;

            entry = item;
            return true;
        }
    }
}