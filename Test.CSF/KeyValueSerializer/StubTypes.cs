using System;
using System.Collections.Generic;
using CSF.Entities;
using CSF.KeyValueSerializer.MappingModel;

namespace Test.CSF.KeyValueSerializer
{
  public class Foo
  {
    public string TestProperty { get; set; }

    public DateTime TestDateTime { get; set; }

    public int TestInteger { get; set; }

    public string TestPropertyTwo { get; set; }
  }

  public class Bar
  {
    public Foo Foo { get; set; }

    public Baz Baz { get; set; }

    public ICollection<Baz> BazCollection { get; set; }

    public ICollection<DateTime> ValueCollection { get; set; }

    public string BarProperty { get; set; }

    public EntityType Entity { get; set; }

    public Bar(string val)
    {
      this.Foo = new Foo();
      this.Baz = new Baz();
      this.BarProperty = val;
    }
  }

  public class Baz
  {
    public ICollection<Bar> TestCollection { get; set; }

    public ICollection<DateTime> TestValueCollection { get; set; }

    public ICollection<EntityType> EntityCollection { get; set; }

    public DateTime BazProperty { get; set; }

    public Baz()
    {
      this.TestCollection = new List<Bar>();
    }
  }

  public class EntityType : Entity<EntityType,int> {}

  public class TestNamingPolicy : DefaultKeyNamingPolicy
  {
    public string TestString { get; set; }

    public TestNamingPolicy(IMapping mapping) : base(mapping) {}
  }

  public class MockClass
  {
    public string PropertyOne { get; set; }

    public int PropertyTwo { get; set; }

    public DateTime PropertyThree { get; set; }

    public Person PropertyFour { get; set; }

    public MockClass NestedClass { get; set; }

    public IList<string> CollectionOne { get; set; }

    public IList<IPerson> CollectionTwo { get; set; }

    public IList<DateTime> CollectionThree { get; set; }
  }

  public interface IPerson : IEntity<Person,uint>
  {
    string Name { get; set; }

    DateTime Birthday { get; set; }

    int Friends { get; set; }
  }

  public class Person : Entity<Person,uint>, IPerson
  {
    public string Name { get; set; }

    public DateTime Birthday { get; set; }

    public int Friends { get; set; }
  }
}

