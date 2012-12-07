using System;
using CSF.KeyValueSerializer.MappingModel;
using CSF.Reflection;
using Moq;
using NUnit.Framework;
using System.Reflection;

namespace Test.CSF.KeyValueSerializer.MappingModel
{
  [TestFixture]
  public class TestDefaultKeyNamingPolicy
  {
    [Test]
    public void TestConstructor()
    {
      var rootMapping = new Mock<IClassMapping<Foo>>();
      DefaultKeyNamingPolicy rule = new DefaultKeyNamingPolicy(rootMapping.Object);

      rootMapping.Setup(x => x.KeyNamingPolicy)
        .Returns(rule);

      Assert.AreSame(rootMapping.Object, rule.AssociatedMapping);
    }

    [Test]
    public void TestGetKeyNameEmpty()
    {
      var rootMapping = new Mock<IClassMapping<Foo>>();
      DefaultKeyNamingPolicy rule = new DefaultKeyNamingPolicy(rootMapping.Object);

      rootMapping.Setup(x => x.KeyNamingPolicy)
        .Returns(rule);

      string name = rule.GetKeyName();
      Assert.IsNotNull(name);
      Assert.IsEmpty(name);
    }

    [Test]
    public void TestGetKeyNameProperty()
    {
      var rootMapping = new Mock<IClassMapping<Foo>>();
      var propertyMapping = new Mock<ISimpleMapping<string>>();
      var prop = Reflect.Property<Foo>(x => x.TestProperty);

      propertyMapping.Setup(x => x.Property)
        .Returns(prop);
      propertyMapping.Setup(x => x.ParentMapping)
        .Returns(rootMapping.Object);

      DefaultKeyNamingPolicy
        rootRule = new DefaultKeyNamingPolicy(rootMapping.Object),
        propRule = new DefaultKeyNamingPolicy(propertyMapping.Object);

      rootMapping.Setup(x => x.KeyNamingPolicy)
        .Returns(rootRule);
      propertyMapping.Setup(x => x.KeyNamingPolicy)
        .Returns(propRule);

      string name = propRule.GetKeyName();
      Assert.IsNotNull(name);
      Assert.AreEqual(prop.Name, name);
    }

    [Test]
    public void TestGetKeyNameNestedProperty()
    {
      var rootMapping = new Mock<IMapping>();
      var propertyMapping = new Mock<IMapping>();
      var nestedMapping = new Mock<IMapping>();

      var prop = Reflect.Property<Bar>(x => x.Foo);
      var nestedProp = Reflect.Property<Foo>(x => x.TestProperty);

      rootMapping.Setup(x => x.ToString()).Returns("Root mapping");
      rootMapping.SetupGet(x => x.Property).Returns((PropertyInfo) null);
      rootMapping.SetupGet(x => x.ParentMapping).Returns((IMapping) null);
      rootMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(rootMapping.Object));

      propertyMapping.Setup(x => x.ToString()).Returns("Property mapping");
      propertyMapping.SetupGet(x => x.Property).Returns(prop);
      propertyMapping.SetupGet(x => x.ParentMapping).Returns(rootMapping.Object);
      propertyMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(propertyMapping.Object));

      nestedMapping.Setup(x => x.ToString()).Returns("Nested mapping");
      nestedMapping.SetupGet(x => x.Property).Returns(nestedProp);
      nestedMapping.SetupGet(x => x.ParentMapping).Returns(propertyMapping.Object);
      nestedMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(nestedMapping.Object));

      Assert.IsNotNull(nestedProp, "Testing-the-test: Nested property not null");
      Assert.IsNotNull(prop, "Testing-the-test: Property not null");

      string name = nestedMapping.Object.KeyNamingPolicy.GetKeyName();
      Assert.IsNotNull(name);
      Assert.AreEqual(String.Format("{0}.{1}", prop.Name, nestedProp.Name), name);
    }

    [Test]
    public void TestGetKeyNameCollection()
    {
      var rootMapping = new Mock<IMapping>();
      var collectionMapping = new Mock<IMapping>();
      var nestedMapping = new Mock<IMapping>();

      var collectionProperty = Reflect.Property<Baz>(x => x.TestCollection);
      var nestedProp = Reflect.Property<Bar>(x => x.BarProperty);

      rootMapping.Setup(x => x.ToString()).Returns("Root mapping");
      rootMapping.SetupGet(x => x.Property).Returns((PropertyInfo) null);
      rootMapping.SetupGet(x => x.ParentMapping).Returns((IMapping) null);
      rootMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(rootMapping.Object));

      collectionMapping.As<ICollectionMapping>().SetupGet(x => x.CollectionKeyType).Returns(CollectionKeyType.Separate);
      collectionMapping.Setup(x => x.ToString()).Returns("Property (collection) mapping");
      collectionMapping.SetupGet(x => x.Property).Returns(collectionProperty);
      collectionMapping.SetupGet(x => x.ParentMapping).Returns(rootMapping.Object);
      collectionMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(collectionMapping.Object));

      nestedMapping.Setup(x => x.ToString()).Returns("Nested mapping");
      nestedMapping.SetupGet(x => x.Property).Returns(nestedProp);
      nestedMapping.SetupGet(x => x.ParentMapping).Returns(collectionMapping.Object);
      nestedMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(nestedMapping.Object));

      Assert.IsNotNull(nestedProp, "Testing-the-test: Nested property not null");
      Assert.IsNotNull(collectionProperty, "Testing-the-test: Property not null");

      string name = nestedMapping.Object.KeyNamingPolicy.GetKeyName(5);
      Assert.IsNotNull(name);
      Assert.AreEqual(String.Format("{0}[5].{1}", collectionProperty.Name, nestedProp.Name), name);
    }

    [Test]
    public void TestGetKeyNameRootCollection()
    {
      var collectionMapping = new Mock<IMapping>();
      var nestedMapping = new Mock<IMapping>();

      var nestedProp = Reflect.Property<Bar>(x => x.BarProperty);

      collectionMapping.As<ICollectionMapping>().SetupGet(x => x.CollectionKeyType).Returns(CollectionKeyType.Separate);
      collectionMapping.Setup(x => x.ToString()).Returns("Property (collection) mapping");
      collectionMapping.SetupGet(x => x.Property).Returns((PropertyInfo) null);
      collectionMapping.SetupGet(x => x.ParentMapping).Returns((IMapping) null);
      collectionMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(collectionMapping.Object));

      nestedMapping.Setup(x => x.ToString()).Returns("Nested mapping");
      nestedMapping.SetupGet(x => x.Property).Returns(nestedProp);
      nestedMapping.SetupGet(x => x.ParentMapping).Returns(collectionMapping.Object);
      nestedMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(nestedMapping.Object));

      Assert.IsNotNull(nestedProp, "Testing-the-test: Nested property not null");

      string name = nestedMapping.Object.KeyNamingPolicy.GetKeyName(5);
      Assert.IsNotNull(name);
      Assert.AreEqual(String.Format("[5].{0}", nestedProp.Name), name);
    }

    [Test]
    public void TestGetKeyNameNestedCollection()
    {
      var rootMapping = new Mock<IMapping>();
      var collectionMapping = new Mock<IMapping>();
      var nestedMapping = new Mock<IMapping>();
      var collection2Mapping = new Mock<IMapping>();
      var nestedMapping2 = new Mock<IMapping>();

      var collection1Property = Reflect.Property<Baz>(x => x.TestCollection);
      var nestedProp = Reflect.Property<Bar>(x => x.Baz);
      var collection2Property = Reflect.Property<Baz>(x => x.TestCollection);
      var nestedProp2 = Reflect.Property<Bar>(x => x.Baz);

      rootMapping.Setup(x => x.ToString()).Returns("Root mapping");
      rootMapping.SetupGet(x => x.Property).Returns((PropertyInfo) null);
      rootMapping.SetupGet(x => x.ParentMapping).Returns((IMapping) null);
      rootMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(rootMapping.Object));

      collectionMapping.As<ICollectionMapping>().SetupGet(x => x.CollectionKeyType).Returns(CollectionKeyType.Separate);
      collectionMapping.Setup(x => x.ToString()).Returns("Property (collection 1) mapping");
      collectionMapping.SetupGet(x => x.Property).Returns(collection1Property);
      collectionMapping.SetupGet(x => x.ParentMapping).Returns(rootMapping.Object);
      collectionMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(collectionMapping.Object));

      nestedMapping.Setup(x => x.ToString()).Returns("Nested mapping");
      nestedMapping.SetupGet(x => x.Property).Returns(nestedProp);
      nestedMapping.SetupGet(x => x.ParentMapping).Returns(collectionMapping.Object);
      nestedMapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(nestedMapping.Object));

      collection2Mapping.As<ICollectionMapping>().SetupGet(x => x.CollectionKeyType).Returns(CollectionKeyType.Separate);
      collection2Mapping.Setup(x => x.ToString()).Returns("Property (collection 2) mapping");
      collection2Mapping.SetupGet(x => x.Property).Returns(collection2Property);
      collection2Mapping.SetupGet(x => x.ParentMapping).Returns(nestedMapping.Object);
      collection2Mapping.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(collection2Mapping.Object));

      nestedMapping2.Setup(x => x.ToString()).Returns("Nested mapping 2");
      nestedMapping2.SetupGet(x => x.Property).Returns(nestedProp2);
      nestedMapping2.SetupGet(x => x.ParentMapping).Returns(collection2Mapping.Object);
      nestedMapping2.SetupGet(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(nestedMapping2.Object));

      string name = nestedMapping2.Object.KeyNamingPolicy.GetKeyName(5, 3);
      Assert.IsNotNull(name);
      Assert.AreEqual(String.Format("{0}[5].{1}.{2}[3].{3}",
                                    collection1Property.Name,
                                    nestedProp.Name,
                                    collection2Property.Name,
                                    nestedProp2.Name),
                      name);
    }

//    [Test]
//    public void TestGetKeyNameCollectionContainer()
//    {
//      var rootMapping = new Mock<IMapping>();
//      var collectionMapping = new Mock<IMapping>();
//      var collection2Mapping = new Mock<IMapping>();
//      var mapAsOne = new Mock<IMapping>();
//      var mapAsTwo = new Mock<IMapping>();
//
//      var firstProperty = Reflect.Property<CollectionContainer>(x => x.FirstCollection);
//      var secondProperty = Reflect.Property<CollectionContainer>(x => x.SecondCollection);
//
//      rootMapping.Setup(x => x.Property).Returns((PropertyInfo) null);
//      rootMapping.Setup(x => x.ParentMapping).Returns((IMapping) null);
//      rootMapping.Setup(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(rootMapping.Object));
//
//      collectionMapping.As<ICollectionMapping>().SetupGet(x => x.CollectionKeyType).Returns(CollectionKeyType.Separate);
//      collectionMapping.Setup(x => x.Property).Returns(firstProperty);
//      collectionMapping.Setup(x => x.ParentMapping).Returns(rootMapping.Object);
//      collectionMapping.Setup(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(collectionMapping.Object));
//
//      collection2Mapping.As<ICollectionMapping>().SetupGet(x => x.CollectionKeyType).Returns(CollectionKeyType.Separate);
//      collection2Mapping.Setup(x => x.Property).Returns(secondProperty);
//      collection2Mapping.Setup(x => x.ParentMapping).Returns(rootMapping.Object);
//      collection2Mapping.Setup(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(collection2Mapping.Object));
//
//      mapAsOne.Setup(x => x.Property).Returns((PropertyInfo) null);
//      mapAsOne.Setup(x => x.ParentMapping).Returns(collectionMapping.Object);
//      mapAsOne.Setup(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(mapAsOne.Object));
//
//      mapAsTwo.Setup(x => x.Property).Returns((PropertyInfo) null);
//      mapAsTwo.Setup(x => x.ParentMapping).Returns(collection2Mapping.Object);
//      mapAsTwo.Setup(x => x.KeyNamingPolicy).Returns(new DefaultKeyNamingPolicy(mapAsTwo.Object));
//
//      string name = mapAsOne.Object.KeyNamingPolicy.GetKeyName(2);
//      Assert.IsNotNull(name);
//
//      Assert.AreNotEqual(String.Format("{0}[{1}]", firstProperty.Name, 2, ))
//    }
  }
}

