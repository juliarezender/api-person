using ApiPerson.Controllers;
using ApiPerson.Models;
using ApiPerson.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using ApiPerson.Models.Context;
using ApiPerson.Services.Implementations;

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
        public void TesteFindByIDChamadaUmaVezQuandoGetPossuirParametrosEPessoaDiferenteDeNull()
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
        public void TestFindAllChamadaUmaVezQuandoGetN�oPossuirParametros()
        {
            // arrange
            var person = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            // act 
            _personController.Get();
            // assert
            _mockPersonService.Verify(Mock => Mock.FindAll(), Times.Once());
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
        public void TesteDeleteChamaUmaVezQuandoUsarMetodoDelete()
        {
            // arrange
            long id = 1;
            // act
            var response = _personController.Delete(id);
            // assert
            _mockPersonService.Verify(mock => mock.Delete(id), Times.Once());
            Assert.IsInstanceOf<NoContentResult>(response);
        }
    }
}