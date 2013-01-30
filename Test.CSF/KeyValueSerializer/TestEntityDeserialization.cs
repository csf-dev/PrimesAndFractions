using System;
using NUnit.Framework;
using CSF.Entities;
using System.Collections.Generic;
using CSF.KeyValueSerializer;
using System.Collections.Specialized;
using CSF.Collections;
using System.Linq;

namespace Test.CSF.KeyValueSerializer
{
  [TestFixture]
  [Category("Integration")]
  public class TestEntityDeserialization
  {
    #region tests

    [Test]
    [Description("This test tries to replicate a possible bug?")]
    public void TestDeserialize()
    {
      var serializer = new ClassKeyValueSerializer<TestEntity>();

      serializer.Map(root => {
        root.Simple(x => x.Id);

        root.Simple(x => x.Name);

        root.Collection(coll => coll.Inners, collMap => {
          collMap.UsingFactory(() => new InnerEntity());

          collMap.Simple(x => x.Id);

          collMap.Simple(x => x.Name);
        });
      });

      var data = new NameValueCollection { { "Id", "5" },
                                           { "Name", "Foo" },
                                           { "Inners[0].Id", "1" },
                                           { "Inners[0].Name", "Foo" },
                                           { "Inners[1].Name", "Baz" },
                                           { "Inners[2].Id", "3" } };

      TestEntity result = serializer.Deserialize(data.ToDictionary());

      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(5, result.Id, "Result ID");
      Assert.AreEqual("Foo", result.Name, "Result name");

      if(result.Inners.Count != 3)
      {
        Console.Error.WriteLine("Wrong count of inner elements found - ones that are found listed:");
        foreach(var inner in result.Inners)
        {
          Console.Error.WriteLine ("Inner found: ID = {0}, Name = {1}", inner.Id, inner.Name);
        }
      }

      Assert.AreEqual(3, result.Inners.Count, "Inner item count");
      Assert.IsTrue(result.Inners.Any(x => x.Id == 1 && x.Name == "Foo"), "First inner match");
      Assert.IsTrue(result.Inners.Any(x => x.Name == "Baz"), "Second inner match");
      Assert.IsTrue(result.Inners.Any(x => x.Id == 3), "Third inner match");
    }

    #endregion

    #region contained type

    public class TestEntity : Entity<TestEntity,ulong>, ITestEntity
    {
      public virtual string Name
      {
        get;
        set;
      }

      public virtual ICollection<IInnerEntity> Inners
      {
        get;
        set;
      }
    }

    public class InnerEntity : Entity<InnerEntity,ulong>, IInnerEntity
    {
      public virtual string Name
      {
        get;
        set;
      }
    }

    public interface ITestEntity : IEntity<TestEntity,ulong>
    {
      string Name { get; set; }

      ICollection<IInnerEntity> Inners { get; set; }
    }

    public interface IInnerEntity : IEntity<InnerEntity,ulong>
    {
      string Name { get; set; }
    }

    #endregion
  }
}

