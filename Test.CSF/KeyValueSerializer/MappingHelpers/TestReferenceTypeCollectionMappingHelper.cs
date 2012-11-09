using System;
using NUnit.Framework;
using Moq;
using CSF.KeyValueSerializer.MappingHelpers;
using CSF.KeyValueSerializer.MappingModel;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF.KeyValueSerializer.MappingHelpers
{
  [TestFixture]
  public class TestReferenceTypeCollectionMappingHelper
  {
    #region general methods

    [Test]
    public void TestUsingFactory()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();
      mapping.SetupProperty(x => x.MapAs);
      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);

      Func<Bar> factory = () => new Bar("Wibble") { BarProperty = "Test!" };
      helper.UsingFactory(factory);

      mapping.VerifySet(x => x.MapAs = It.IsAny<IClassMapping<Bar>>());
      Assert.IsNotNull(mapping.Object.MapAs.FactoryMethod, "Factory method not null");
      Assert.AreSame(factory, mapping.Object.MapAs.FactoryMethod, "Factory method is correct");
    }

    [Test]
    public void TestCollectionNamingPolicy()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();
      mapping.SetupProperty(x => x.MapAs);
      mapping.Setup(x => x.AttachKeyNamingPolicy<TestNamingPolicy>());
      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);

      helper.CollectionNamingPolicy<TestNamingPolicy>();

      mapping.Verify(x => x.AttachKeyNamingPolicy<TestNamingPolicy>());
    }

    [Test]
    public void TestCollectionNamingPolicyWithFactory()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();
      mapping.SetupProperty(x => x.MapAs);
      mapping.Setup(x => x.AttachKeyNamingPolicy<TestNamingPolicy>(It.IsAny<Func<IMapping,TestNamingPolicy>>()));
      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);

      helper.CollectionNamingPolicy<TestNamingPolicy>(map => new TestNamingPolicy(map) { TestString = "Test!" });

      mapping.Verify(x => x.AttachKeyNamingPolicy<TestNamingPolicy>(It.IsAny<Func<IMapping,TestNamingPolicy>>()));
    }

    #endregion

    #region mapping the type itself

    [Test]
    public void TestMapAsSimple()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();
      mapping.Setup(x => x.ParentMapping);
      mapping.Setup(x => x.Property);
      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);

      var simple = helper.Simple();

      Assert.IsNotNull(simple, "Simple mapping helper is not null");
      Assert.IsNotNull(mapping.Object.MapAs.MapAs, "Map-as of parent object is not null");
      Assert.IsTrue(mapping.Object.MapAs.MapAs is ISimpleMapping<Bar>, "Map-as is of correct type");
    }

    [Test]
    public void TestMapAsComposite()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();
      mapping.Setup(x => x.ParentMapping);
      mapping.Setup(x => x.Property);
      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);

      var composite = helper.Composite();

      Assert.IsNotNull(composite, "Composite mapping helper is not null");
      Assert.IsNotNull(mapping.Object.MapAs.MapAs, "Map-as of parent object is not null");
      Assert.IsTrue(mapping.Object.MapAs.MapAs is ICompositeMapping<Bar>, "Map-as is of correct type");
    }

    [Test]
    public void TestMapAsEntity()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<EntityType>>();
      mapping.Setup(x => x.ParentMapping);
      mapping.Setup(x => x.Property);
      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,EntityType> helper = new ReferenceTypeCollectionMappingHelper<Baz,EntityType>(mapping.Object);

      var entity = helper.Entity<EntityType,int>();

      Assert.IsNotNull(entity, "Entity mapping helper is not null");
      Assert.IsNotNull(mapping.Object.MapAs.MapAs, "Map-as of parent object is not null");
      Assert.IsTrue(mapping.Object.MapAs.MapAs is ISimpleMapping<EntityType>, "Map-as is of correct type");
    }

    #endregion

    #region adding mappings

    [Test]
    public void TestSimple()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();

      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);
      var simple = helper.Simple(x => x.BarProperty);

      Assert.IsNotNull(simple);
      Assert.AreEqual(1, mapping.Object.MapAs.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.MapAs.Mappings.First() is ISimpleMapping<string>, "Mapping is of correct type");
    }

    [Test]
    public void TestSimpleTwice()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();

      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);
      var simple = helper.Simple(x => x.BarProperty);
      var simple2 = helper.Simple(x => x.BarProperty);

      Assert.IsNotNull(simple);
      Assert.IsNotNull(simple2);
      Assert.AreEqual(1,
                      mapping.Object.MapAs.Mappings.Count,
                      "Correct count of contained mappings, only one mapping was created.");
      Assert.IsTrue(mapping.Object.MapAs.Mappings.First() is ISimpleMapping<string>, "Mapping is of correct type");
    }

    [Test]
    public void TestComposite()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();

      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);
      var comp = helper.Composite(x => x.BarProperty);

      Assert.IsNotNull(comp);
      Assert.AreEqual(1, mapping.Object.MapAs.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.MapAs.Mappings.First() is ICompositeMapping<string>, "Mapping is of correct type");
    }

    [Test]
    public void TestCollection()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();

      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);
      helper.Collection(x => x.BazCollection, m => {});

      Assert.AreEqual(1, mapping.Object.MapAs.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.MapAs.Mappings.First() is IReferenceTypeCollectionMapping<Baz>,
                    "Mapping is of correct type");
    }

    [Test]
    public void TestValueCollection()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();

      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);
      helper.ValueCollection(x => x.ValueCollection, m => {});

      Assert.AreEqual(1, mapping.Object.MapAs.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.MapAs.Mappings.First() is IValueTypeCollectionMapping<DateTime>,
                    "Mapping is of correct type");
    }

    [Test]
    public void TestClass()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();

      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);
      helper.Class(x => x.Foo, m => {});

      Assert.AreEqual(1, mapping.Object.MapAs.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.MapAs.Mappings.First() is IClassMapping<Foo>,
                    "Mapping is of correct type");
    }

    [Test]
    public void TestEntity()
    {
      var mapping = new Mock<IReferenceTypeCollectionMapping<Bar>>();

      mapping.SetupProperty(x => x.MapAs);

      ReferenceTypeCollectionMappingHelper<Baz,Bar> helper = new ReferenceTypeCollectionMappingHelper<Baz,Bar>(mapping.Object);
      helper.Entity<EntityType,int>(x => x.Entity);

      Assert.AreEqual(1, mapping.Object.MapAs.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.MapAs.Mappings.First() is ISimpleMapping<EntityType>,
                    "Mapping is of correct type");
    }

    #endregion
  }
}

