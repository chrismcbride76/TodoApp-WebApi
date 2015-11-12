using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;
using Todo.Api.Models;

namespace Todo.Api.Test
{
    [TestClass]
    public class InMemoryTodoRepositoryShould
    {
        [TestMethod]
        public void SetIncrementedIdWhenAddingTodo()
        {
            var todo = new TodoModel {task = "My task"};

            var repository = new InMemoryTodoRepository();
            todo = repository.Add(todo);
            todo.id.ShouldEqual(1);

            todo = repository.Add(todo);
            todo.id.ShouldEqual(2);

            todo = repository.Add(todo);
            todo.id.ShouldEqual(3);
        }

        [TestMethod]
        public void GetTodo()
        {
            var todo = new TodoModel { task = "My task" };

            var repository = new InMemoryTodoRepository();
            repository.Add(todo);
            repository.Get(todo.id).ShouldEqual(todo);
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
                repository.Add(new TodoModel { task = i.ToString() });
            }

            repository.GetAll().Count().ShouldEqual(5);
        }

        [TestMethod]
        public void RemoveTodo()
        {
            var repository = new InMemoryTodoRepository();
            var addedTodo = repository.Add(new TodoModel());
            repository.Delete(addedTodo.id).ShouldBeTrue();
            repository.Get(addedTodo.id).ShouldBeNull();

            repository.Delete(addedTodo.id).ShouldBeFalse();
            repository.Get(addedTodo.id).ShouldBeNull();
        }

        [TestMethod]
        public void UpdateTodo()
        {
            var repository = new InMemoryTodoRepository();

            var originalTodo = new TodoModel
            {
                completed = true,
                deadlineUtc = DateTime.UtcNow,
                moreDetails = "more",
                task = "task"
            };

            originalTodo = repository.Add(originalTodo);

            var updatedTodo = new TodoModel
            {
                id = originalTodo.id,
                completed = false,
                deadlineUtc = DateTime.Now,
                moreDetails = "more details",
                task = "task info"
            };

            repository.Update(updatedTodo).ShouldBeTrue();

            var updatedOriginal = repository.Get(originalTodo.id);
            updatedOriginal.id.ShouldEqual(updatedTodo.id);
            updatedOriginal.completed.ShouldEqual(updatedTodo.completed);
            updatedOriginal.deadlineUtc.ShouldEqual(updatedTodo.deadlineUtc);
            updatedOriginal.moreDetails.ShouldEqual(updatedTodo.moreDetails);
            updatedOriginal.task.ShouldEqual(updatedTodo.task);
        }
    }
}
