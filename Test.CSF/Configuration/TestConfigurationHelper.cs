using System;
using NUnit.Framework;
using CSF.Configuration;

namespace Test.CSF.Configuration
{
  [TestFixture]
  public class TestConfigurationHelper
  {
    [Test]
    public void TestGetDefaultConfigurationPath()
    {
      string defaultPath = ConfigurationHelper.GetDefaultConfigurationPath<MockConfigurationSection>();
      Assert.AreEqual("Test/CSF/Configuration/MockConfigurationSection", defaultPath);
    }

    [Test]
    public void TestGetSection()
    {
      MockConfigurationSection section = ConfigurationHelper.GetSection<MockConfigurationSection>();
      Assert.IsNotNull(section, "Section is not null");
      Assert.AreEqual("Configured value", section.MockProperty, "Property value correct");
    }
  }
}

