using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Cli;
using CSF.Reflection;
using NUnit.Framework;

namespace Test.CSF.Cli
{
  [TestFixture]
  public class TestPosixParameterParser
  {
    #region tests relating to adding parameters
    
    [Test]
    public void TestAddParameter()
    {
      PosixParameterParser parser = new PosixParameterParser();
      
      parser
        .AddParameter("f", "foo", "test.foo")
        .AddParameter("b", null, "test.b")
        .AddParameter(null, "bar", "test.bar");
      
      Assert.AreEqual(3, parser.RegisteredParameters.Count, "Correct count of parameters");
    }
    
    [Test]
    public void TestAddParameters()
    {
      PosixParameterParser parser = new PosixParameterParser();
      
      parser.AddParameters<SampleClass>();
      
      Assert.AreEqual(2, parser.RegisteredParameters.Count, "Correct count of parameters");
      
      object identifier = StaticReflectionUtility.GetMember<SampleClass>(x => x.PropertyOne);
      IParameter testParameter = (from x in parser.RegisteredParameters
                                  where x.Identifier == identifier
                                  select x).FirstOrDefault();
      
      Assert.AreEqual(3, testParameter.ShortNames.Count, "Correct count of short names");
    }
    
    #endregion
    
    #region contained type
    
    class SampleClass : ParsedParameters<SampleClass>
    {
      [Parameter(ParameterBehaviour.ValueRequired, ShortName = "n")]
      [ParameterName("l")]
      [ParameterName("m")]
      public int PropertyOne
      {
        get {
          IParameter<int> parameter = this.Get<int>(x => x.PropertyOne);
          
          return (parameter != null)? parameter.GetValue() : 0;
        }
      }
      
      [Parameter(ParameterBehaviour.Switch, ShortName = "s", LongName = "switch")]
      [ParameterName("a-switch", IsLongName = true)]
      [ParameterName("another-switch", IsLongName = true)]
      public bool PropertyTwo
      {
        get {
          return this.Contains(x => x.PropertyOne);
        }
      }
      
      public SampleClass(IDictionary<object, IParameter> parameters,
                         IList<string> remainingArguments) : base(parameters, remainingArguments) {}
    }
    
    #endregion
  }
}

