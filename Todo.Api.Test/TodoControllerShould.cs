﻿using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Should;
using Todo.Api.Controllers;
using Todo.Api.Models;

namespace Todo.Api.Test
{
    [TestClass]
    public class TodoControllerShould
    {
        [TestMethod]
        public void AddTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            ToDo todo = new ToDo
            {
                DeadlineUtc = DateTime.UtcNow,
                IsCompleted = false,
                Task = "Some task"
            };

            mockRepository.Setup(x => x.Add(todo)).Callback(() => todo.Id = 1).Returns(todo);

            var response = controller.Post(todo) as CreatedAtRouteNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.RouteName.ShouldEqual("DefaultApi");
            response.RouteValues["Id"].ShouldEqual(response.Content.Id);
            response.Content.ShouldEqual(todo);

            mockRepository.Verify(x => x.Add(todo), Times.Once());
        }

        [TestMethod]
        public void GetTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            ToDo todo = new ToDo
            {
                Id = 3,
                DeadlineUtc = DateTime.UtcNow,
                IsCompleted = false,
                Task = "Some task"
            };

            mockRepository.Setup(x => x.Get(todo.Id)).Returns(todo);

            var response = controller.Get(todo.Id) as OkNegotiatedContentResult<ToDo>;
            response.ShouldNotBeNull();
            response.Content.ShouldEqual(todo);
        }

        [TestMethod]
        public void GetAllTodos()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            var allTodos = new List<ToDo>();
            mockRepository.Setup(x => x.GetAll()).Returns(allTodos);

            var response = controller.Get() as OkNegotiatedContentResult<IEnumerable<ToDo>>;
            response.ShouldNotBeNull();
            response.Content.ShouldEqual(allTodos);
        }


        [TestMethod]
        public void ReturnNotFoundIfCantFindTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            var response = controller.Get(1) as NotFoundResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutIdDoesntMatchTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            var response = controller.Put(5, new ToDo {Id = 4}) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
        }

        [TestMethod]
        public void ReturnBadRequestWhenPutCantUpdate()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            mockRepository.Setup(x => x.Update(It.IsAny<ToDo>())).Returns(false);

            var response = controller.Put(5, new ToDo { Id = 5 }) as BadRequestErrorMessageResult;
            response.ShouldNotBeNull();
            mockRepository.Verify(x => x.Update(It.IsAny<ToDo>()), Times.Once);
        }

        [TestMethod]
        public void UpdateTodoOnPut()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            var todo = new ToDo
            {
                Id = 5,
                Task = "My Task"
            };

            var updatedTodo = new ToDo
            {
                Id = 5,
                Task = "Updated Task"
            };

            mockRepository.Setup(x => x.Update(updatedTodo)).Returns(true);

            var response = controller.Put(todo.Id, updatedTodo) as OkResult;
            response.ShouldNotBeNull();

            mockRepository.Verify(x => x.Update(updatedTodo), Times.Once);
        }

        [TestMethod]
        public void DeleteTodo()
        {
            Mock<IToDoRepository> mockRepository = new Mock<IToDoRepository>();
            TodoController controller = new TodoController(mockRepository.Object);

            mockRepository.Setup(x => x.Remove(1));

            controller.Delete(1);
            mockRepository.Verify(x => x.Remove(1), Times.Once);
        }
    }
}
