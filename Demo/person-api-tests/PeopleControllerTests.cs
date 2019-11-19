using System.Linq;
using NUnit.Framework;
using person_api.Controllers;

namespace person_api_tests
{
    public class PeopleControllerTests
    {
        PeopleController controller;

        [SetUp]
        public void Setup()
        {
            controller = new PeopleController(new FakePeopleProvider());
        }

        [Test]
        public void GetPeople_ReturnsAllValues()
        {
            var result = controller.Get();
            Assert.AreEqual(9, result.Count());
        }

        [Test]
        public void GetPerson_OnValidValue_ReturnsPerson()
        {
            var result = controller.Get(2);
            Assert.AreEqual(2, result.Id);
        }

        [Test]
        public void GetPerson_OnInvalidValue_ReturnsNull()
        {
            var result = controller.Get(-1);
            Assert.IsNull(result);
        }
    }
}