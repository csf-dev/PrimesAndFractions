using System;
using System.Collections.Generic;

namespace Test.CSF.Collections.Serialization.MappingModel
{
  public class Foo
  {
    public string TestProperty { get; set; }
  }

  public class Bar
  {
    public Foo Foo { get; set; }

    public Baz Baz { get; set; }

    public string BarProperty { get; set; }

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

    public Baz()
    {
      this.TestCollection = new List<Bar>();
    }
  }
}

