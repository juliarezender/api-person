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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace ApiPerson.Test
{
    [TestFixture]
    public class Tests
    {
        private readonly ILogger<PersonController> _logger;
        private PersonController _personController;
        private Mock<IPersonService> _mockPersonService;
        private PersonServiceImplementation personServiceImplementation;

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
        public void TestGetQuandoNãoPossuirParametrosEListaNaoVazia()
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
        public void TestGetQuandoNãoPossuirParametrosEListaVazia()
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

        [Test]
        public void TesteMetodoDeleteQuandoIdNaoExiste()
        {
            // arrange
            long id = 1;
            // act
            var response = _personController.Delete(id);
            // assert
            Assert.IsInstanceOf<NotFoundResult>(response);
        }
        public static Mock<DbSet<T>> MockDbSet<T>(List<T> inputDbSetContent) where T : class
        {
            var DbSetContent = inputDbSetContent.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(DbSetContent.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(DbSetContent.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(DbSetContent.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => inputDbSetContent.GetEnumerator());
            dbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>((s) => inputDbSetContent.Add(s));
            dbSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>((s) => inputDbSetContent.Remove(s));
            return dbSet;
        }

        [Test]
        public void TesteRetornoDoMetodoCreate()
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

            var mockContext = new Mock<PersonContext>();
            var mockPessoaSet = MockDbSet(new List<Person> { person });
            mockContext.Setup(mock => mock.Persons).Returns(mockPessoaSet.Object);
            var personServiceImplementation = new PersonServiceImplementation(mockContext.Object);
            // act
            var newPerson = personServiceImplementation.Create(person);

            // assert
            Assert.AreEqual(newPerson, person);
        }

        [Test]
        public void TesteRetornoDoMetodoUpdate()
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

            var mockContext = new Mock<PersonContext>();
            var mockPessoaSet = MockDbSet(new List<Person> {  });
            mockContext.Setup(mock => mock.Persons).Returns(mockPessoaSet.Object);
            var personServiceImplementation = new PersonServiceImplementation(mockContext.Object);
            // act
            var newPerson = personServiceImplementation.Update(person);

            // assert
            Assert.AreEqual(newPerson, person);
        }

        [Test]
        public void TesteRetornoDoMetodoFindById()
        {
            // arrange
            var person1 = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };

            var mockContext = new Mock<PersonContext>();
            var mockPessoaSet = MockDbSet(new List<Person> { person1 });
            mockContext.Setup(mock => mock.Persons).Returns(mockPessoaSet.Object);
            var personServiceImplementation = new PersonServiceImplementation(mockContext.Object);

            var newPerson = personServiceImplementation.FindByID(person1.Id);

            // assert
            Assert.AreEqual(newPerson, person1);
        }

        [Test]
        public void TesteRetornoDoMetodoFindAll()
        {
            // arrange

            var person1 = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            var person2 = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            var listPerson = new List<Person>();
            listPerson.Add(person1);
            listPerson.Add(person2);

            var mockContext = new Mock<PersonContext>();
            var mockPessoaSet = MockDbSet(new List<Person> { person1, person2 });
            mockContext.Setup(mock => mock.Persons).Returns(mockPessoaSet.Object);
            var personServiceImplementation = new PersonServiceImplementation(mockContext.Object);

            var newPersons = personServiceImplementation.FindAll();

            // assert
            Assert.AreEqual(newPersons, listPerson);
        }

        [Test]
        public void TesteExcecaoNoMetodoCreate()
        {
            // arrange
            var person1 = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };

            var mockContext = new Mock<PersonContext>();
            var mockPessoaSet = MockDbSet(new List<Person> {  });
            mockContext.Setup(mock => mock.Persons).Returns(mockPessoaSet.Object);
            var personServiceImplementation = new PersonServiceImplementation(mockContext.Object);
            
            mockContext.Setup(mock => mock.Add(person1)).Throws(new Exception());

            // assert
            Assert.Throws<Exception>(() => personServiceImplementation.Create(person1));
        }

        [Test]
        public void TesteExcecaoNoMetodoDelete()
        {
            // arrange
            long id = 1;
            var person1 = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            var mockContext = new Mock<PersonContext>();
            var mockPessoaSet = MockDbSet(new List<Person> { person1 });
            mockContext.Setup(mock => mock.Persons).Returns(mockPessoaSet.Object);
            var personServiceImplementation = new PersonServiceImplementation(mockContext.Object);

            mockContext.Setup(mock => mock.Remove(person1)).Throws(new Exception());

            // assert
            Assert.Throws<Exception>(() => personServiceImplementation.Delete(id));
        }

        [Test]
        public void TesteExcecaoNoMetodoUpdate()
        {
            // arrange
            long id = 1;
            var person1 = new Person
            {
                Id = 1,
                FirstName = "Julia",
                LastName = "Rezende",
                Address = "Lagoa da Prata",
                Gender = "Feminino"
            };
            var mockContext = new Mock<PersonContext>();
            var mockPessoaSet = MockDbSet(new List<Person> { person1 });
            mockContext.Setup(mock => mock.Persons).Returns(mockPessoaSet.Object);
            var personServiceImplementation = new PersonServiceImplementation(mockContext.Object);

            mockContext.Setup(mock => mock.Entry(person1)).Throws(new Exception());

            // assert
            Assert.Throws<Exception>(() => personServiceImplementation.Update(person1));
        }
    }
}