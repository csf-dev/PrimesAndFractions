using System;
using NUnit.Framework;
using CSF.Entities;
using Moq;

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestIIdentityExtensions
  {
    #region tests

    [Test]
    public void TestUnwrap()
    {
      IIdentity<Person> identity = Identity.Create<Person,uint>(5);

      var service = new Mock<IIdentityUnwrappingService>();
      service
        .Setup(x => x.Unwrap<Person>(identity))
        .Returns(new Person() { Id = 5, Name = "Joe Bloggs" });

      Person testPerson = identity.Unwrap(service.Object);
      Assert.IsNotNull(testPerson);
      Assert.AreEqual("Joe Bloggs", testPerson.Name, "Correct person returned");
    }

    #endregion

    #region stub entity type

    class Person : Entity<Person, uint>
    {
      public virtual string Name
      {
        get;
        set;
      }
    }

    #endregion
  }
}

