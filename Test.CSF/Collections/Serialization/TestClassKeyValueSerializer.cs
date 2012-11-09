using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Entities;
using CSF.Collections.Serialization;
using CSF.Collections.Serialization.MappingModel;
using System.Linq;
using CSF.Reflection;
using CSF.Patterns.IoC;
using Moq;
using NHibernate;
using CSF.Data.NHibernate;


namespace Test.CSF.Collections.Serialization
{
  [TestFixture]
  [Category("Integration")]
  public class TestClassKeyValueSerializer
  {
    #region mapping

    [Test]
    public void TestMap()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Simple(x => x.PropertyOne);

        root.Composite(x => x.PropertyThree)
          .Component("Year", m => m.Serialize(d => d.Year.ToString()))
          .Component("Month", m => m.Serialize(d => d.Month.ToString()))
          .Component("Day", m => m.Serialize(d => d.Day.ToString()))
          .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                            Int32.Parse(data["Month"]),
                                            Int32.Parse(data["Day"])));

        root.Collection<string>(x => x.CollectionOne, m => m.Simple());

        root.ValueCollection<DateTime>(x => x.CollectionThree, m => {
          m.Composite()
            .Component("Year", map => map.Serialize(d => d.Year.ToString()))
            .Component("Month", map => map.Serialize(d => d.Month.ToString()))
            .Component("Day", map => map.Serialize(d => d.Day.ToString()))
            .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                              Int32.Parse(data["Month"]),
                                              Int32.Parse(data["Day"])));
        });

        root.Class(x => x.PropertyFour, m => {
          m.Simple(p => p.Name);

          m.Simple(p => p.Friends);

          m.Composite(p => p.Birthday)
            .Component("Year", map => map.Serialize(d => d.Year.ToString()))
            .Component("Month", map => map.Serialize(d => d.Month.ToString()))
            .Component("Day", map => map.Serialize(d => d.Day.ToString()))
            .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                              Int32.Parse(data["Month"]),
                                              Int32.Parse(data["Day"])));
        });
      });

      Assert.IsNotNull(serializer.RootMapping, "Root mapping not null");
      Assert.IsInstanceOfType(typeof(IClassMapping<MockClass>),
                              serializer.RootMapping,
                              "Root mapping correct type");
      Assert.AreEqual(5,
                      ((IClassMapping<MockClass>) serializer.RootMapping).Mappings.Count,
                      "Correct count of mappings");

      var collectionOneProp = Reflect.Property<MockClass>(x => x.CollectionOne);
      IClassMapping<MockClass> rootMapping = (IClassMapping<MockClass>) serializer.RootMapping;
      Assert.AreEqual(1,
                      rootMapping.Mappings.Where(x => x.Property == collectionOneProp).Count(),
                      "Mapping present for collection one.");
      Assert.IsInstanceOfType(typeof(IReferenceTypeCollectionMapping<string>),
                              rootMapping.Mappings.Where(x => x.Property == collectionOneProp).First(),
                              "Mapping for collection one is right type");

      Assert.AreEqual("PropertyFour.Friends",
                      serializer.RootMapping
                        .GetMapping<MockClass>(x => x.PropertyFour)
                        .GetMapping<Person>(x => x.Friends)
                        .GetKeyName(),
                      "Key name for friends property is correct");

      Assert.AreEqual("PropertyFour.BirthdayYear",
                      serializer.RootMapping
                        .GetMapping<MockClass>(x => x.PropertyFour)
                        .GetMapping<Person>(x => x.Birthday)
                        .GetKeyName("Year"),
                      "Key name for birthday year component is correct");

      Assert.AreEqual("PropertyFour.BirthdayMonth",
                      serializer.RootMapping
                        .GetMapping<MockClass>(x => x.PropertyFour)
                        .GetMapping<Person>(x => x.Birthday)
                        .GetKeyName("Month"),
                      "Key name for birthday month component is correct");
    }

    #endregion

    #region deserialization

    [Test]
    public void TestDeserialize()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Simple(x => x.PropertyOne);

        root.Composite(x => x.PropertyThree)
          .Component("Year", m => m.Serialize(d => d.Year.ToString()))
          .Component("Month", m => m.Serialize(d => d.Month.ToString()))
          .Component("Day", m => m.Serialize(d => d.Day.ToString()))
          .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                            Int32.Parse(data["Month"]),
                                            Int32.Parse(data["Day"])));

        root.Collection<string>(x => x.CollectionOne, m => m.Simple());

        root.ValueCollection<DateTime>(x => x.CollectionThree, m => {
          m.ArrayStyleList();
          m.Composite()
            .Component("Year", map => map.Serialize(d => d.Year.ToString()))
            .Component("Month", map => map.Serialize(d => d.Month.ToString()))
            .Component("Day", map => map.Serialize(d => d.Day.ToString()))
            .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                              Int32.Parse(data["Month"]),
                                              Int32.Parse(data["Day"])));
        });

        root.Class(x => x.PropertyFour, m => {
          m.Simple(p => p.Name);

          m.Simple(p => p.Friends);

          m.Composite(p => p.Birthday)
            .Component("Year", map => map.Serialize(d => d.Year.ToString()))
            .Component("Month", map => map.Serialize(d => d.Month.ToString()))
            .Component("Day", map => map.Serialize(d => d.Day.ToString()))
            .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                              Int32.Parse(data["Month"]),
                                              Int32.Parse(data["Day"])));
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("PropertyOne", "Test value for property one");
      collection.Add("PropertyThreeYear", "2012");
      collection.Add("PropertyThreeMonth", "11");
      collection.Add("PropertyThreeDay", "2");
      collection.Add("CollectionOne[0]", "Sample value 1");
      collection.Add("CollectionOne[1]", "Sample value 2");
      collection.Add("CollectionOne[2]", "Sample value 3");
      collection.Add("CollectionThree[0]Year", "2010");
      collection.Add("CollectionThree[0]Month", "2");
      collection.Add("CollectionThree[0]Day", "2");
      collection.Add("CollectionThree[1]Year", "2000");
      collection.Add("CollectionThree[1]Month", "1");
      collection.Add("CollectionThree[1]Day", "1");
      collection.Add("PropertyFour.Name", "Craig Fowler");
      collection.Add("PropertyFour.Friends", "20");
      collection.Add("PropertyFour.BirthdayYear", "1982");
      collection.Add("PropertyFour.BirthdayMonth", "4");
      collection.Add("PropertyFour.BirthdayDay", "6");

      MockClass output = serializer.Deserialize(collection);

      Assert.AreEqual("Test value for property one", output.PropertyOne);
      Assert.AreEqual(new DateTime(2012,11,2), output.PropertyThree);
      Assert.AreEqual(3, output.CollectionOne.Count);
      Assert.IsTrue(output.CollectionOne.Any(x => x == "Sample value 1"));
      Assert.IsTrue(output.CollectionOne.Any(x => x == "Sample value 2"));
      Assert.IsTrue(output.CollectionOne.Any(x => x == "Sample value 3"));
      Assert.AreEqual(2, output.CollectionThree.Count);
      Assert.IsTrue(output.CollectionThree.Any(x => x.Equals(new DateTime(2010,2,2))));
      Assert.IsTrue(output.CollectionThree.Any(x => x.Equals(new DateTime(2000,1,1))));
      Assert.AreEqual("Craig Fowler", output.PropertyFour.Name);
      Assert.AreEqual(20, output.PropertyFour.Friends);
      Assert.AreEqual(new DateTime(1982,4,6), output.PropertyFour.Birthday);
    }

    [Test]
    public void TestDeserializeNestedCollectionClass()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(x => {
        x.Collection<Bar>(prop => prop.TestCollection, y => {
          y.UsingFactory(() => new Bar("String property"));
          y.Class(bar => bar.Baz, z => {
            z.Composite()
              .Component("Year", s => s.Serialize(v => v.BazProperty.Year.ToString()))
              .Component("Month", s => s.Serialize(v => v.BazProperty.Month.ToString()))
              .Deserialize(v => new Baz() {
                BazProperty = new DateTime(Int32.Parse(v["Year"]), Int32.Parse(v["Month"]), 1)
              });
          });
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("TestCollection[0].BazYear", "2010");
      collection.Add("TestCollection[0].BazMonth", "10");
      collection.Add("TestCollection[1].BazYear", "2011");
      collection.Add("TestCollection[1].BazMonth", "11");
      collection.Add("TestCollection[2].BazYear", "2012");
      collection.Add("TestCollection[2].BazMonth", "12");

      Baz result = serializer.Deserialize(collection);

      Assert.IsNotNull(result);
      Assert.AreEqual(3, result.TestCollection.Count, "Correct count of items");
      Assert.IsTrue(result.TestCollection.Any(x => x.Baz.BazProperty.Equals(new DateTime(2010, 10, 1))),
                    "First result present");
      Assert.IsTrue(result.TestCollection.Any(x => x.Baz.BazProperty.Equals(new DateTime(2011, 11, 1))),
                    "Second result present");
      Assert.IsTrue(result.TestCollection.Any(x => x.Baz.BazProperty.Equals(new DateTime(2012, 12, 1))),
                    "Third result present");
    }

    [Test]
    public void TestDeserializeNestedClass()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Class(first => first.NestedClass, firstMap => {
          firstMap.Class(second => second.NestedClass, secondMap => {
            secondMap.Simple(x => x.PropertyOne);
          });
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("NestedClass.NestedClass.PropertyOne", "Hello!");

      MockClass result = serializer.Deserialize(collection);
      Assert.IsNotNull(result);
      Assert.AreEqual("Hello!", result.NestedClass.NestedClass.PropertyOne, "Property value is correct");
    }

    [Test]
    public void TestDeserializeEntity()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Entity(x => x.PropertyFour).Deserialize(s => new Person() { Id = UInt32.Parse(s) });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("PropertyFour", "2");

      MockClass result = serializer.Deserialize(collection);
      Assert.IsNotNull(result, "Result not null");
      Assert.IsNotNull(result.PropertyFour, "Entity not null");
      Assert.AreEqual(2, result.PropertyFour.GetIdentity().Value, "Identity correct");
    }

    [Test]
    public void TestDeserializeEntityCollection()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Collection(x => x.CollectionTwo, c => {
          c.Entity<Person,uint>().Deserialize(s => new Person() { Id = UInt32.Parse(s) });
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("CollectionTwo[0]", "1");
      collection.Add("CollectionTwo[1]", "3");

      MockClass result = serializer.Deserialize(collection);
      Assert.IsNotNull(result, "Result not null");
      Assert.IsNotNull(result.CollectionTwo, "Collection not null");
      Assert.AreEqual(2, result.CollectionTwo.Count, "Collection correct size");
      Assert.IsTrue(result.CollectionTwo.Any(x => x.Id == 1), "First result present");
      Assert.IsTrue(result.CollectionTwo.Any(x => x.Id == 3), "Second result present");
    }

    [Test]
    public void TestDeserializeNestedCollection()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(root => {
        root.Collection(rootProp => rootProp.TestCollection, rootMap => {
          rootMap.UsingFactory(() => new Bar("Test value"));
          rootMap.Collection(firstProp => firstProp.BazCollection, firstMap => {
            firstMap.Composite(secondProp => secondProp.BazProperty)
              .Component("Year", s => s.Serialize(v => v.Year.ToString()))
              .Component("Month", s => s.Serialize(v => v.Month.ToString()))
              .Deserialize(v => new DateTime(Int32.Parse(v["Year"]), Int32.Parse(v["Month"]), 1));
          });
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("TestCollection[0].BazCollection[0].BazPropertyYear", "2010");
      collection.Add("TestCollection[0].BazCollection[0].BazPropertyMonth", "1");
      collection.Add("TestCollection[1].BazCollection[0].BazPropertyYear", "2011");
      collection.Add("TestCollection[1].BazCollection[0].BazPropertyMonth", "1");
      collection.Add("TestCollection[0].BazCollection[1].BazPropertyYear", "2012");
      collection.Add("TestCollection[0].BazCollection[1].BazPropertyMonth", "1");
      collection.Add("TestCollection[1].BazCollection[1].BazPropertyYear", "2013");
      collection.Add("TestCollection[1].BazCollection[1].BazPropertyMonth", "1");

      Baz result = serializer.Deserialize(collection);

      Assert.IsNotNull(result, "Result is not null");
      Assert.IsNotNull(result.TestCollection, "Test collection is not null");
      Assert.AreEqual(2, result.TestCollection.Count, "Test collection correct length");
      Assert.IsNotNull(result.TestCollection.Skip(0).Take(1).First().BazCollection,
                       "1st Inner collection not null");
      Assert.IsNotNull(result.TestCollection.Skip(1).Take(1).First().BazCollection,
                       "2nd inner collection not null");
      Assert.AreEqual(2,
                      result.TestCollection.Skip(0).Take(1).First().BazCollection.Count,
                      "1st Inner collection correct length");
      Assert.AreEqual(2,
                      result.TestCollection.Skip(1).Take(1).First().BazCollection.Count,
                      "2nd inner collection correct length");
      Assert.IsTrue(result.TestCollection.Skip(0).Take(1).First().BazCollection.Any(x => x.BazProperty.Year == 2010),
                    "1st Inner collection contains 2010");
      Assert.IsTrue(result.TestCollection.Skip(0).Take(1).First().BazCollection.Any(x => x.BazProperty.Year == 2012),
                    "1st Inner collection contains 2012");
      Assert.IsTrue(result.TestCollection.Skip(1).Take(1).First().BazCollection.Any(x => x.BazProperty.Year == 2011),
                    "2nd Inner collection contains 2011");
      Assert.IsTrue(result.TestCollection.Skip(1).Take(1).First().BazCollection.Any(x => x.BazProperty.Year == 2013),
                    "2nd Inner collection contains 2013");
    }

    [Test]
    public void TestDeserializeCommaSeparatedEntityCollection()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Collection(x => x.CollectionTwo, c => {
          c.CommaSeparatedList();
          c.Entity<Person,uint>().Deserialize(s => new Person() { Id = UInt32.Parse(s) });
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("CollectionTwo", "1,3");

      MockClass result = serializer.Deserialize(collection);
      Assert.IsNotNull(result, "Result not null");
      Assert.IsNotNull(result.CollectionTwo, "Collection not null");
      Assert.AreEqual(2, result.CollectionTwo.Count, "Collection correct size");
      Assert.IsTrue(result.CollectionTwo.Any(x => x.Id == 1), "First result present");
      Assert.IsTrue(result.CollectionTwo.Any(x => x.Id == 3), "Second result present");
    }

    [Test]
    [ExpectedException]
    public void TestDeserializeCommaSeparatedCompositeNotPermitted()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(root => {
        root.ValueCollection(x => x.TestValueCollection, c => {
          c.CommaSeparatedList();
          c.Composite()
            .Component("Year", s => s.Serialize(v => v.Year.ToString()))
            .Component("Month", s => s.Serialize(v => v.Month.ToString()))
            .Deserialize(v => new DateTime(Int32.Parse(v["Year"]), Int32.Parse(v["Month"]), 1));
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("TestValueCollectionYear", "2010,2012");
      collection.Add("TestValueCollectionMonth", "1,2");

      serializer.Deserialize(collection);
    }

    [Test]
    public void TestDeserializeCommaSeparatedSimple()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(root => {
        root.ValueCollection(x => x.TestValueCollection, c => {
          c.CommaSeparatedList();
          c.Simple().Deserialize(s => DateTime.Parse(s));
        });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("TestValueCollection", "2010-6-10,2012-11-06");

      Baz result = serializer.Deserialize(collection);
      Assert.IsNotNull(result, "Result not null");
      Assert.IsNotNull(result.TestValueCollection, "Collection not null");
      Assert.AreEqual(2, result.TestValueCollection.Count, "Collection correct size");
      Assert.IsTrue(result.TestValueCollection.Any(x => x.Year == 2010 && x.Month == 6), "First result present");
      Assert.IsTrue(result.TestValueCollection.Any(x => x.Year == 2012 && x.Month == 11), "Second result present");
    }

    #endregion

    #region serialization

    [Test]
    public void TestSerialize()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Simple(x => x.PropertyOne);

        root.Composite(x => x.PropertyThree)
          .Component("Year", m => m.Serialize(d => d.Year.ToString()))
          .Component("Month", m => m.Serialize(d => d.Month.ToString()))
          .Component("Day", m => m.Serialize(d => d.Day.ToString()))
          .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                            Int32.Parse(data["Month"]),
                                            Int32.Parse(data["Day"])));

        root.Collection<string>(x => x.CollectionOne, m => m.Simple());

        root.ValueCollection<DateTime>(x => x.CollectionThree, m => {
          m.ArrayStyleList();
          m.Composite()
            .Component("Year", map => map.Serialize(d => d.Year.ToString()))
            .Component("Month", map => map.Serialize(d => d.Month.ToString()))
            .Component("Day", map => map.Serialize(d => d.Day.ToString()))
            .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                              Int32.Parse(data["Month"]),
                                              Int32.Parse(data["Day"])));
        });

        root.Class(x => x.PropertyFour, m => {
          m.Simple(p => p.Name);

          m.Simple(p => p.Friends);

          m.Composite(p => p.Birthday)
            .Component("Year", map => map.Serialize(d => d.Year.ToString()))
            .Component("Month", map => map.Serialize(d => d.Month.ToString()))
            .Component("Day", map => map.Serialize(d => d.Day.ToString()))
            .Deserialize(data => new DateTime(Int32.Parse(data["Year"]),
                                              Int32.Parse(data["Month"]),
                                              Int32.Parse(data["Day"])));
        });
      });

      var mock = new MockClass();

      mock.PropertyOne = "Test value for property one";
      mock.PropertyThree = new DateTime(2012, 11, 2);
      mock.CollectionOne = new string[] { "Sample value 1", "Sample value 2", "Sample value 3" };
      mock.CollectionThree = new DateTime[] { new DateTime(2012, 2, 2), new DateTime(2000, 1, 1) };
      mock.PropertyFour = new Person() {
        Name = "Craig Fowler",
        Friends = 20,
        Birthday = new DateTime(1982, 4, 6)
      };

      var output = serializer.Serialize(mock);

      Assert.IsTrue(output["PropertyOne"] == "Test value for property one");
      Assert.IsTrue(output["PropertyThreeYear"] == "2012");
      Assert.IsTrue(output["PropertyThreeMonth"] == "11");
      Assert.IsTrue(output["PropertyThreeDay"] == "2");
      Assert.IsTrue(output["CollectionOne[0]"] == "Sample value 1");
      Assert.IsTrue(output["CollectionOne[1]"] == "Sample value 2");
      Assert.IsTrue(output["CollectionOne[2]"] == "Sample value 3");
      Assert.IsTrue(output["CollectionThree[0]Year"] == "2012");
      Assert.IsTrue(output["CollectionThree[0]Month"] == "2");
      Assert.IsTrue(output["CollectionThree[0]Day"] == "2");
      Assert.IsTrue(output["CollectionThree[1]Year"] == "2000");
      Assert.IsTrue(output["CollectionThree[1]Month"] == "1");
      Assert.IsTrue(output["CollectionThree[1]Day"] == "1");
      Assert.IsTrue(output["PropertyFour.Name"] == "Craig Fowler");
      Assert.IsTrue(output["PropertyFour.Friends"] == "20");
      Assert.IsTrue(output["PropertyFour.BirthdayYear"] == "1982");
      Assert.IsTrue(output["PropertyFour.BirthdayMonth"] == "4");
      Assert.IsTrue(output["PropertyFour.BirthdayDay"] == "6");
    }

    [Test]
    public void TestSerializeNestedCollectionClass()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(x => {
        x.Collection<Bar>(prop => prop.TestCollection, y => {
          y.UsingFactory(() => new Bar("String property"));
          y.Class(bar => bar.Baz, z => {
            z.Composite()
              .Component("Year", s => s.Serialize(v => v.BazProperty.Year.ToString()))
              .Component("Month", s => s.Serialize(v => v.BazProperty.Month.ToString()))
              .Deserialize(v => new Baz() {
                BazProperty = new DateTime(Int32.Parse(v["Year"]), Int32.Parse(v["Month"]), 1)
              });
          });
        });
      });

      var data = new Baz();
      data.TestCollection = new Bar[] {
        new Bar("String") {
          Baz = new Baz() { BazProperty = new DateTime(2010, 10, 1) }
        },
        new Bar("String") {
          Baz = new Baz() { BazProperty = new DateTime(2011, 11, 1) }
        },
        new Bar("String") {
          Baz = new Baz() { BazProperty = new DateTime(2012, 12, 1) }
        }
      };

      var output = serializer.Serialize(data);

      Assert.IsTrue(output["TestCollection[0].BazYear"] == "2010");
      Assert.IsTrue(output["TestCollection[0].BazMonth"] == "10");
      Assert.IsTrue(output["TestCollection[1].BazYear"] == "2011");
      Assert.IsTrue(output["TestCollection[1].BazMonth"] == "11");
      Assert.IsTrue(output["TestCollection[2].BazYear"] == "2012");
      Assert.IsTrue(output["TestCollection[2].BazMonth"] == "12");
    }

    [Test]
    public void TestSerializeNestedClass()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Class(first => first.NestedClass, firstMap => {
          firstMap.Class(second => second.NestedClass, secondMap => {
            secondMap.Simple(x => x.PropertyOne);
          });
        });
      });

      var data = new MockClass() {
        NestedClass = new MockClass() {
          NestedClass = new MockClass() { PropertyOne = "Hello!" }
        }
      };

      var output = serializer.Serialize(data);

      Assert.IsTrue(output["NestedClass.NestedClass.PropertyOne"] == "Hello!");
    }

    [Test]
    public void TestSerializeEntity()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Entity(x => x.PropertyFour).Deserialize(s => new Person() { Id = UInt32.Parse(s) });
      });

      var data = new MockClass() { PropertyFour = new Person() { Id = 2 } };

      var result = serializer.Serialize(data);

      Assert.IsTrue(result["PropertyFour"] == "2");
    }

    [Test]
    public void TestSerializeEntityCollection()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Collection(x => x.CollectionTwo, c => {
          c.Entity<Person,uint>().Deserialize(s => new Person() { Id = UInt32.Parse(s) });
        });
      });

      var data = new MockClass() {
        CollectionTwo = new Person[] {
          new Person() { Id = 1 },
          new Person() { Id = 3 }
        }
      };

      var result = serializer.Serialize(data);

      Assert.IsTrue(result["CollectionTwo[0]"] == "1");
      Assert.IsTrue(result["CollectionTwo[1]"] == "3");
    }

    [Test]
    public void TestSerializeNestedCollection()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(root => {
        root.Collection(rootProp => rootProp.TestCollection, rootMap => {
          rootMap.UsingFactory(() => new Bar("Test value"));
          rootMap.Collection(firstProp => firstProp.BazCollection, firstMap => {
            firstMap.Composite(secondProp => secondProp.BazProperty)
              .Component("Year", s => s.Serialize(v => v.Year.ToString()))
              .Component("Month", s => s.Serialize(v => v.Month.ToString()))
              .Deserialize(v => new DateTime(Int32.Parse(v["Year"]), Int32.Parse(v["Month"]), 1));
          });
        });
      });

      var data = new Baz() {
        TestCollection = new Bar[] {
          new Bar("Test value") {
            BazCollection = new Baz[] {
              new Baz() { BazProperty = new DateTime(2010, 1, 1) },
              new Baz() { BazProperty = new DateTime(2012, 1, 1) },
            }
          },
          new Bar("Test value") {
            BazCollection = new Baz[] {
              new Baz() { BazProperty = new DateTime(2011, 1, 1) },
              new Baz() { BazProperty = new DateTime(2013, 1, 1) },
            }
          }
        }
      };

      var result = serializer.Serialize(data);

      Assert.IsTrue(result["TestCollection[0].BazCollection[0].BazPropertyYear"] == "2010");
      Assert.IsTrue(result["TestCollection[0].BazCollection[0].BazPropertyMonth"] == "1");
      Assert.IsTrue(result["TestCollection[0].BazCollection[1].BazPropertyYear"] == "2012");
      Assert.IsTrue(result["TestCollection[0].BazCollection[1].BazPropertyMonth"] == "1");
      Assert.IsTrue(result["TestCollection[1].BazCollection[0].BazPropertyYear"] == "2011");
      Assert.IsTrue(result["TestCollection[1].BazCollection[0].BazPropertyMonth"] == "1");
      Assert.IsTrue(result["TestCollection[1].BazCollection[1].BazPropertyYear"] == "2013");
      Assert.IsTrue(result["TestCollection[1].BazCollection[1].BazPropertyMonth"] == "1");
    }

    [Test]
    public void TestSerializeCommaSeparatedEntityCollection()
    {
      var serializer = new ClassKeyValueSerializer<MockClass>();

      serializer.Map(root => {
        root.Collection(x => x.CollectionTwo, c => {
          c.CommaSeparatedList();
          c.Entity<Person,uint>().Deserialize(s => new Person() { Id = UInt32.Parse(s) });
        });
      });

      var data = new MockClass() {
        CollectionTwo = new Person[] {
          new Person() { Id = 1 },
          new Person() { Id = 3 }
        }
      };

      var result = serializer.Serialize(data);

      Assert.IsTrue(result["CollectionTwo"] == "1,3");
    }

    [Test]
    [ExpectedException]
    public void TestSerializeCommaSeparatedCompositeNotPermitted()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(root => {
        root.ValueCollection(x => x.TestValueCollection, c => {
          c.CommaSeparatedList();
          c.Composite()
            .Component("Year", s => s.Serialize(v => v.Year.ToString()))
            .Component("Month", s => s.Serialize(v => v.Month.ToString()))
            .Deserialize(v => new DateTime(Int32.Parse(v["Year"]), Int32.Parse(v["Month"]), 1));
        });
      });

      var data = new Baz() {
        TestValueCollection = new DateTime[] {
          new DateTime(2010,1,1),
          new DateTime(2012,2,1)
        }
      };

      serializer.Serialize(data);
    }

    [Test]
    public void TestSerializeCommaSeparatedSimple()
    {
      var serializer = new ClassKeyValueSerializer<Baz>();

      serializer.Map(root => {
        root.ValueCollection(x => x.TestValueCollection, c => {
          c.CommaSeparatedList();
          c.Simple()
            .Deserialize(s => DateTime.Parse(s))
            .Serialize(d => d.ToString("yyyy-M-d"));
        });
      });

      var data = new Baz() {
        TestValueCollection = new DateTime[] {
          new DateTime(2010,6,10),
          new DateTime(2012,11,6)
        }
      };

      var result = serializer.Serialize(data);

      Assert.IsTrue(result["TestValueCollection"] == "2010-6-10,2012-11-6");
    }

    #endregion
  }
}

