using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq.Expressions;
using CSF.Entities;
using CSF.Collections.Serialization;


namespace Test.CSF.Collections.Serialization
{
  [TestFixture]
  public class TestKeyValueSerializer
  {
    [Test]
    [Ignore("This is not a real test")]
    public void TestMappingAPI()
    {
      IKeyValueSerializer<MockClass> serializer;

      serializer.Map(x => {
        x.Simple(p => p.PropertyOne);

        x.Simple(p => p.PropertyTwo)
          .NamingRule(KeyName.Is("property_two"))
          .Deserialize(s => (int) Convert.ChangeType(String.Concat ("1", s), typeof(int)))
          .Serialize(v => (v - 10).ToString());

        x.Composite(p => p.PropertyThree)
          .NamingRule(KeyName.SuffixComponentIdentifier('_'))
          .Component("year", c => {
            c.Serialize(v => v.Year.ToString());
          })
          .Component("month", c => {
            c.Serialize(v => v.Month.ToString());
          })
          .Deserialize(c => new DateTime(Int32.Parse(c[1]), Int32.Parse(c[2]), 1));

        x.Class(p => p.PropertyFour, m => {
          m.Simple(y => y.Name);
          m.Composite(y => y.Birthday)
            .Component(1, c => {
              c.NamingRule(KeyName.WithSuffix("_year"));
              c.Serialize(v => v.Year.ToString());
            })
            .Component(2, c => {
              c.NamingRule(KeyName.WithSuffix("_month"));
              c.Serialize(v => v.Month.ToString());
            })
            .Deserialize(c => new DateTime(Int32.Parse(c[1]), Int32.Parse(c[2]), 1));
        });

        x.Collection(p => p.CollectionTwo, m => {
          m.UsingNumericSuffix("_");
          m.Simple(y => y.Name)
            .Mandatory();
          m.Simple(y => y.Friends);
        });

        x.Collection(p => p.CollectionOne, m => {
          m.UsingAggregateSeparator(",");
          m.Mandatory();
          m.Simple()
            .NamingRule(KeyName.Is("strVal"));
        });

        x.ValueCollection(p => p.CollectionThree, m => {
          m.UsingAggregateSeparator(",");
          m.Composite()
            .Component(1, c => {
              c.NamingRule(KeyName.WithSuffix("_year"));
              c.Serialize(v => v.Year.ToString());
            })
            .Component(2, c => {
              c.NamingRule(KeyName.WithSuffix("_month"));
              c.Serialize(v => v.Month.ToString());
            })
            .Deserialize(c => new DateTime(Int32.Parse(c[1]), Int32.Parse(c[2]), 1));
        });

        x.Collection(prop => prop.CollectionTwo, m => {
          m.UsingAggregateSeparator(",");
          m.Entity<Person,uint>()
            .NamingRule(KeyName.Is("person_id"));
        });

        x.UsingFactory(() => new MockClass() { PropertyOne = "Beans!" });
        x.UsingFlag("action", "Create");
      });
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

