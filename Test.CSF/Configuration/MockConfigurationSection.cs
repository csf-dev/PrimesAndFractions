using System;
using System.Configuration;

namespace Test.CSF.Configuration
{
  public class MockConfigurationSection : ConfigurationSection
  {
    [ConfigurationProperty(@"MockProperty", IsRequired = false, DefaultValue = "")]
    public virtual string MockProperty
    {
      get {
        return (string) this["MockProperty"];
      }
      set {
        this["MockProperty"] = value;
      }
    }
  }
}

