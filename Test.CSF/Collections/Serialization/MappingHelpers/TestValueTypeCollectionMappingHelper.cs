using System;
using NUnit.Framework;
using Moq;
using CSF.Collections.Serialization.MappingHelpers;
using CSF.Collections.Serialization.MappingModel;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF.Collections.Serialization.MappingHelpers
{
  [TestFixture]
  public class TestValueTypeCollectionMappingHelper
  {
    [Test]
    public void TestSimple()
    {
      var mapping = new Mock<IValueTypeCollectionMapping<DateTime>>();
      mapping.SetupProperty(x => x.MapAs);
      ValueTypeCollectionMappingHelper<Bar,DateTime> helper = new ValueTypeCollectionMappingHelper<Bar,DateTime>(mapping.Object);

      helper.Simple();

      Assert.IsNotNull(mapping.Object.MapAs, "Map-as not null");
      Assert.IsInstanceOfType(typeof(ISimpleMapping<DateTime>), mapping.Object.MapAs, "Correct type");
    }

    [Test]
    public void TestComposite()
    {
      var mapping = new Mock<IValueTypeCollectionMapping<DateTime>>();
      mapping.SetupProperty(x => x.MapAs);
      ValueTypeCollectionMappingHelper<Bar,DateTime> helper = new ValueTypeCollectionMappingHelper<Bar,DateTime>(mapping.Object);

      helper.Composite();

      Assert.IsNotNull(mapping.Object.MapAs, "Map-as not null");
      Assert.IsInstanceOfType(typeof(ICompositeMapping<DateTime>), mapping.Object.MapAs, "Correct type");
    }
  }
}

