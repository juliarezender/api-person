using ApiPerson.Controllers;
using ApiPerson.Models;
using ApiPerson.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using ApiPerson.Models.Context;
using ApiPerson.Services.Implementations;
using System.Collections.Generic;

namespace ApiPerson.Test
{
    [TestFixture]
    public class Tests
    {
        private readonly ILogger<PersonController> _logger;
        private PersonController _personController;
        private Mock<IPersonService> _mockPersonService;

        [SetUp]
        public void Setup()
        {
            _mockPersonService = new Mock<IPersonService>();
            _personController = new PersonController(_logger, _mockPersonService.Object);
        }

        [Test]
        public void TesteGetQuandoPossuirParametrosEPessoaDiferenteDeNull()
        {
            // arrange
            long id = 1;
            var person = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            _mockPersonService.Setup(mock => mock.FindByID(id)).Returns(person);
            // act 
            var response = _personController.Get(id);
            // assert
            _mockPersonService.Verify(Mock => Mock.FindByID(id), Times.Once());
            Assert.IsInstanceOf<OkObjectResult>(response);
        }
        [Test]
        public void TesteGetQuandoPossuirParametrosEPessoaIgualANull()
        {
            // arrange
            long id = 1;
            Person person = null;
            _mockPersonService.Setup(mock => mock.FindByID(id)).Returns(person);
            // act 
            var response = _personController.Get(id);
            // assert
            Assert.IsInstanceOf<NotFoundResult>(response);
        }

        [Test]
        public void TestGetQuandoN�oPossuirParametrosEListaNaoVazia()
        {
            // arrange
            var listPerson = new List<Person>();
            var person = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            listPerson.Add(person);
            _mockPersonService.Setup(mock => mock.FindAll()).Returns(listPerson);
            // act 
            var response = _personController.Get();
            // assert
            _mockPersonService.Verify(Mock => Mock.FindAll(), Times.Once());
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public void TestGetQuandoN�oPossuirParametrosEListaVazia()
        {
            // arrange
            var listPerson = new List<Person>();
            _mockPersonService.Setup(mock => mock.FindAll()).Returns(listPerson);
            // act 
            var response = _personController.Get();
            // assert
            _mockPersonService.Verify(Mock => Mock.FindAll(), Times.Once());
            Assert.IsInstanceOf<NoContentResult>(response);
        }
        [Test]
        public void TesteCreateChamadaUmaVezQuandoUsarMetodoPost()
        {
            // arrange
            var person = new Person
            {
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            // act
            var response = _personController.Post(person);
            // assert
            _mockPersonService.Verify(mock => mock.Create(person), Times.Once());
            Assert.IsInstanceOf<OkObjectResult>(response);
        }

        [Test]
        public void TesteMetodoPostRetornandoBadResult()
        {
            // arrange
            Person person = null;
            // act
            var response = _personController.Post(person);
            // assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public void TesteUpdateChamaUmaVezQuandoUsarMetodoPut()
        {
            // arrange
            var person = new Person
            {
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            // act
            var response = _personController.Put(person);
            // assert
            _mockPersonService.Verify(mock => mock.Update(person), Times.Once());
            Assert.IsInstanceOf<OkObjectResult>(response);
        }


        [Test]
        public void TesteMetodoPutRetornandoBadRequest()
        {
            // arrange
            Person person = null;
            // act
            var response = _personController.Put(person);
            // assert
            Assert.IsInstanceOf<BadRequestResult>(response);
        }

        [Test]
        public void TesteMetodoDeleteQuandoIdExiste()
        {
            // arrange
            long id = 1;
            var person = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            _mockPersonService.Setup(mock => mock.FindByID(id)).Returns(person);
            // act
            var response = _personController.Delete(id);
            // assert
            _mockPersonService.Verify(mock => mock.Delete(id), Times.Once());
            Assert.IsInstanceOf<NoContentResult>(response);
        }

        public void TesteMetodoDeleteQuandoIdNaoExiste()
        {
            // arrange
            long id = 1;
            // act
            var response = _personController.Delete(id);
            // assert
            Assert.IsInstanceOf<NotFoundResult>(response);
        }
    }
}