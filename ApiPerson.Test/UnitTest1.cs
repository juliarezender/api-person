using Moq;
using NUnit.Framework;

namespace ApiPerson.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var mockPersonService = new Mock<IPersonService>();
        }
    }
}