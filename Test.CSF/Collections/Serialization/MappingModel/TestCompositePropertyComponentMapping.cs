using System;
using NUnit.Framework;
using CSF.Collections.Serialization.MappingModel;
using Moq;

namespace Test.CSF.Collections.Serialization.MappingModel
{
  [TestFixture]
  public class TestCompositePropertyComponentMapping
  {
    [Test]
    public void TestGetKeyName()
    {
      var parent = new Mock<ICompositeMapping<DateTime>>();
      var policy = new Mock<IKeyNamingPolicy>();
      CompositePropertyComponentMapping<DateTime> mapping;

      mapping = new CompositePropertyComponentMapping<DateTime>(parent.Object, "Year");

      parent.Setup(x => x.KeyNamingPolicy).Returns(policy.Object);
      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("TestDate");

      Assert.AreEqual("TestDateYear", mapping.GetKeyName());
    }
  }
}

