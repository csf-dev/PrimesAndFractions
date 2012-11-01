using System;
using System.Collections.Generic;
using CSF.Entities;
using CSF.Collections.Serialization.MappingModel;

namespace Test.CSF.Collections.Serialization
{
  public class Foo
  {
    public string TestProperty { get; set; }

    public DateTime TestDateTime { get; set; }

    public int TestInteger { get; set; }
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

    public Baz()
    {
      this.TestCollection = new List<Bar>();
    }
  }

  public class EntityType : Entity<EntityType,int> {}

  public class TestNamingPolicy : KeyNamingPolicy
  {
    public string TestString { get; set; }

    public TestNamingPolicy(IMapping mapping) : base(mapping) {}
  }
}

