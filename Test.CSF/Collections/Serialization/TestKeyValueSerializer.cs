using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Entities;
using CSF.Collections.Serialization;
using CSF.Collections.Serialization.MappingModel;
using System.Linq;
using CSF.Reflection;


namespace Test.CSF.Collections.Serialization
{
  [TestFixture]
  public class TestKeyValueSerializer
  {
    [Test]
    [Category("Integration")]
    public void TestMap()
    {
      var serializer = new KeyValueSerializer<MockClass>();

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

      var collectionOneProp = StaticReflectionUtility.GetProperty<MockClass>(x => x.CollectionOne);
      IClassMapping<MockClass> rootMapping = (IClassMapping<MockClass>) serializer.RootMapping;
      Assert.AreEqual(1,
                      rootMapping.Mappings.Where(x => x.Property == collectionOneProp).Count(),
                      "Mapping present for collection one.");
      Assert.IsInstanceOfType(typeof(IReferenceTypeCollectionMapping<string>),
                              rootMapping.Mappings.Where(x => x.Property == collectionOneProp).First(),
                              "Mapping for collection one is right type");
    }

    [Test]
    [Category("Integration")]
    public void TestDeserialize()
    {
      var serializer = new KeyValueSerializer<MockClass>();

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

    #region contained types

    public class MockClass
    {
      public string PropertyOne {
        get;
        set;
      }

      public int PropertyTwo {
        get;
        set;
      }

      public DateTime PropertyThree {
        get;
        set;
      }

      public Person PropertyFour {
        get;
        set;
      }

      public IList<string> CollectionOne {
        get;
        set;
      }

      public IList<Person> CollectionTwo {
        get;
        set;
      }

      public IList<DateTime> CollectionThree {
        get;
        set;
      }
    }

    public class Person : Entity<Person,uint>
    {
      public string Name {
        get;
        set;
      }

      public DateTime Birthday {
        get;
        set;
      }

      public int Friends {
        get;
        set;
      }
    }

    #endregion
  }
}

