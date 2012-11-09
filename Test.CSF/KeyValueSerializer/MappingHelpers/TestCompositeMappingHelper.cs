using System;
using NUnit.Framework;
using Moq;
using CSF.KeyValueSerializer.MappingHelpers;
using CSF.KeyValueSerializer.MappingModel;
using System.Collections.Generic;

namespace Test.CSF.KeyValueSerializer.MappingHelpers
{
  [TestFixture]
  public class TestCompositeMappingHelper
  {
    [Test]
    public void TestComponent()
    {
      var mapping = new Mock<ICompositeMapping<DateTime>>();

      mapping.SetupProperty(x => x.Components, new Dictionary<object, ICompositeComponentMapping<DateTime>>());
      CompositeMappingHelper<Baz,DateTime> helper = new CompositeMappingHelper<Baz,DateTime>(mapping.Object);

      helper.Component("Year", m => {});

      Assert.AreEqual(1, mapping.Object.Components.Count, "Correct count of composite mappings");
      Assert.IsTrue(mapping.Object.Components.ContainsKey("Year"), "Component contained with correct key");
    }
  }
}

