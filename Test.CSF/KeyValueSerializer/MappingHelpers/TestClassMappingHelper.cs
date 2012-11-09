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
  public class TestClassMappingHelper
  {
    #region general methods

    [Test]
    public void TestUsingFactory()
    {
      var mapping = new Mock<IClassMapping<Foo>>();
      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);

      helper.UsingFactory(() => new Foo() { TestProperty = "This is a test" });

      mapping.VerifySet(x => x.FactoryMethod = It.IsAny<Func<Foo>>());
    }

    [Test]
    public void TestNamingPolicy()
    {
      var mapping = new Mock<IClassMapping<Foo>>();
      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);
      mapping.Setup(x => x.AttachKeyNamingPolicy<TestNamingPolicy>());

      helper.NamingPolicy<TestNamingPolicy>();

      mapping.Verify(x => x.AttachKeyNamingPolicy<TestNamingPolicy>());
    }

    [Test]
    public void TestNamingPolicyWithFactory()
    {
      var mapping = new Mock<IClassMapping<Foo>>();
      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);
      mapping.Setup(x => x.AttachKeyNamingPolicy<TestNamingPolicy>(It.IsAny<Func<IMapping,TestNamingPolicy>>()));

      helper.NamingPolicy<TestNamingPolicy>(map => new TestNamingPolicy(map) { TestString = "Test!" });

      mapping.Verify(x => x.AttachKeyNamingPolicy<TestNamingPolicy>(It.IsAny<Func<IMapping,TestNamingPolicy>>()));
    }

    #endregion

    #region mapping the type itself

    [Test]
    public void TestMapAsSimple()
    {
      var mapping = new Mock<IClassMapping<Foo>>();
      mapping.Setup(x => x.ParentMapping);
      mapping.Setup(x => x.Property);
      mapping.SetupProperty(x => x.MapAs);

      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);

      var simple = helper.Simple();

      Assert.IsNotNull(simple, "Simple mapping helper is not null");
      Assert.IsNotNull(mapping.Object.MapAs, "Map-as of parent object is not null");
      Assert.IsTrue(mapping.Object.MapAs is ISimpleMapping<Foo>, "Map-as is of correct type");
    }

    [Test]
    public void TestMapAsComposite()
    {
      var mapping = new Mock<IClassMapping<Foo>>();
      mapping.Setup(x => x.ParentMapping);
      mapping.Setup(x => x.Property);
      mapping.SetupProperty(x => x.MapAs);

      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);

      var composite = helper.Composite();

      Assert.IsNotNull(composite, "Composite mapping helper is not null");
      Assert.IsNotNull(mapping.Object.MapAs, "Map-as of parent object is not null");
      Assert.IsTrue(mapping.Object.MapAs is ICompositeMapping<Foo>, "Map-as is of correct type");
    }

    [Test]
    public void TestMapAsEntity()
    {
      var mapping = new Mock<IClassMapping<EntityType>>();
      mapping.Setup(x => x.ParentMapping);
      mapping.Setup(x => x.Property);
      mapping.SetupProperty(x => x.MapAs);

      ClassMappingHelper<EntityType> helper = new ClassMappingHelper<EntityType>(mapping.Object);

      var entity = helper.Entity<EntityType,int>();

      Assert.IsNotNull(entity, "Entity mapping helper is not null");
      Assert.IsNotNull(mapping.Object.MapAs, "Map-as of parent object is not null");
      Assert.IsTrue(mapping.Object.MapAs is ISimpleMapping<EntityType>, "Map-as is of correct type");
    }

    #endregion

    #region adding mappings

    [Test]
    public void TestSimple()
    {
      var mapping = new Mock<IClassMapping<Foo>>();

      mapping.SetupProperty(x => x.Mappings, new List<IMapping>());

      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);
      var simple = helper.Simple(x => x.TestInteger);

      Assert.IsNotNull(simple);
      Assert.AreEqual(1, mapping.Object.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.Mappings.First() is ISimpleMapping<int>, "Mapping is of correct type");
    }

    [Test]
    public void TestSimpleTwice()
    {
      var mapping = new Mock<IClassMapping<Foo>>();

      mapping.SetupProperty(x => x.Mappings, new List<IMapping>());

      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);
      var simple = helper.Simple(x => x.TestInteger);
      var simple2 = helper.Simple(x => x.TestInteger);

      Assert.IsNotNull(simple);
      Assert.IsNotNull(simple2);
      Assert.AreEqual(1,
                      mapping.Object.Mappings.Count,
                      "Correct count of contained mappings, only one mapping was created.");
      Assert.IsTrue(mapping.Object.Mappings.First() is ISimpleMapping<int>, "Mapping is of correct type");
    }

    [Test]
    public void TestComposite()
    {
      var mapping = new Mock<IClassMapping<Foo>>();

      mapping.SetupProperty(x => x.Mappings, new List<IMapping>());

      ClassMappingHelper<Foo> helper = new ClassMappingHelper<Foo>(mapping.Object);
      var comp = helper.Composite(x => x.TestDateTime);

      Assert.IsNotNull(comp);
      Assert.AreEqual(1, mapping.Object.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.Mappings.First() is ICompositeMapping<DateTime>, "Mapping is of correct type");
    }

    [Test]
    public void TestCollection()
    {
      var mapping = new Mock<IClassMapping<Baz>>();

      mapping.SetupProperty(x => x.Mappings, new List<IMapping>());

      ClassMappingHelper<Baz> helper = new ClassMappingHelper<Baz>(mapping.Object);
      helper.Collection(x => x.TestCollection, m => {});

      Assert.AreEqual(1, mapping.Object.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.Mappings.First() is IReferenceTypeCollectionMapping<Bar>,
                    "Mapping is of correct type");
    }

    [Test]
    public void TestValueCollection()
    {
      var mapping = new Mock<IClassMapping<Baz>>();

      mapping.SetupProperty(x => x.Mappings, new List<IMapping>());

      ClassMappingHelper<Baz> helper = new ClassMappingHelper<Baz>(mapping.Object);
      helper.ValueCollection(x => x.TestValueCollection, m => {});

      Assert.AreEqual(1, mapping.Object.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.Mappings.First() is IValueTypeCollectionMapping<DateTime>,
                    "Mapping is of correct type");
    }

    [Test]
    public void TestClass()
    {
      var mapping = new Mock<IClassMapping<Bar>>();

      mapping.SetupProperty(x => x.Mappings, new List<IMapping>());

      ClassMappingHelper<Bar> helper = new ClassMappingHelper<Bar>(mapping.Object);
      helper.Class(x => x.Foo, m => {});

      Assert.AreEqual(1, mapping.Object.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.Mappings.First() is IClassMapping<Foo>,
                    "Mapping is of correct type");
    }

    [Test]
    public void TestEntity()
    {
      var mapping = new Mock<IClassMapping<Bar>>();

      mapping.SetupProperty(x => x.Mappings, new List<IMapping>());

      ClassMappingHelper<Bar> helper = new ClassMappingHelper<Bar>(mapping.Object);
      helper.Entity<EntityType,int>(x => x.Entity);

      Assert.AreEqual(1, mapping.Object.Mappings.Count, "Correct count of contained mappings");
      Assert.IsTrue(mapping.Object.Mappings.First() is ISimpleMapping<EntityType>,
                    "Mapping is of correct type");
    }

    #endregion
  }
}

