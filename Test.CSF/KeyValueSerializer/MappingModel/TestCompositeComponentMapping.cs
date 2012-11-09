using System;
using NUnit.Framework;
using CSF.KeyValueSerializer.MappingModel;
using Moq;

namespace Test.CSF.KeyValueSerializer.MappingModel
{
  [TestFixture]
  public class TestCompositeComponentMapping
  {
    [Test]
    public void TestGetKeyName()
    {
      var parent = new Mock<ICompositeMapping<DateTime>>();
      var policy = new Mock<IKeyNamingPolicy>();
      CompositeComponentMapping<DateTime> mapping;

      mapping = new CompositeComponentMapping<DateTime>(parent.Object, "Year");

      parent.Setup(x => x.KeyNamingPolicy).Returns(policy.Object);
      policy.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("TestDate");

      Assert.AreEqual("TestDateYear", mapping.GetKeyName());
    }
  }
}

