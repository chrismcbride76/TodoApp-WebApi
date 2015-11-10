using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using Todo.Api.Models;

namespace Todo.Api.Test
{
    // TODO: cleanup tests so that they test only what is necessary
    [TestClass]
    public class InMemoryTodoRepositoryShould
    {
        [TestMethod]
        public void SetIdWhenAddingTodo()
        {
            var todo = new ToDo
            {
                Task = "My task"
            };

            var repository = new InMemoryTodoRepository();
            todo = repository.Add(todo);
            todo.Id.ShouldEqual(1);

            todo = repository.Add(todo);
            todo.Id.ShouldEqual(2);

            todo = repository.Add(todo);
            todo.Id.ShouldEqual(3);
        }

        [TestMethod]
        public void GetTodo()
        {
            var todo = new ToDo
            {
                Task = "MyTask"
            };

            var repository = new InMemoryTodoRepository();
            repository.Add(todo);
            repository.Get(todo.Id).ShouldEqual(todo);
        }

        [TestMethod]
        public void ReturnNullIfCantGetTodo()
        {
            var repository = new InMemoryTodoRepository();
            repository.Get(5).ShouldBeNull();
        }

        [TestMethod]
        public void GetAllTodos()
        {
            var repository = new InMemoryTodoRepository();
            for (int i = 0; i < 5; i++)
            {
                repository.Add(new ToDo { Task = i.ToString() });
            }

            repository.GetAll().Count().ShouldEqual(5);
        }

        public void RemoveTodo()
        {
            var repository = new InMemoryTodoRepository();
            var addedTodo = repository.Add(new ToDo());
            repository.Remove(addedTodo.Id);
            repository.Get(addedTodo.Id).ShouldBeNull();

            repository.Remove(addedTodo.Id);
            repository.Get(addedTodo.Id).ShouldBeNull();
        }
    }
}
