using System;
using NUnit.Framework;
using Moq;
using CSF.Collections.Serialization.MappingHelpers;
using CSF.Collections.Serialization.MappingModel;

namespace Test.CSF.Collections.Serialization.MappingHelpers
{
  [TestFixture]
  public class TestMappingHelper
  {
    [Test]
    public void TestCreateNamingPolicy()
    {
      var mapping = new Mock<IMapping>();
      IKeyNamingPolicy policy = MappingHelper.CreateNamingPolicy<TestNamingPolicy>(mapping.Object);

      Assert.IsNotNull(policy, "Policy is not null");
      Assert.AreSame(mapping.Object, policy.AssociatedMapping, "Associated mapping correct");
      Assert.IsInstanceOfType(typeof(TestNamingPolicy), policy, "Correct type");
    }

    [Test]
    public void TestCreateNamingPolicyCustom()
    {
      var mapping = new Mock<IMapping>();
      IKeyNamingPolicy policy = MappingHelper.CreateNamingPolicy<TestNamingPolicy>(mapping.Object,
                                                                                   m => new TestNamingPolicy(m) { TestString = "Test!" });

      Assert.IsNotNull(policy, "Policy is not null");
      Assert.AreSame(mapping.Object, policy.AssociatedMapping, "Associated mapping correct");
      Assert.IsInstanceOfType(typeof(TestNamingPolicy), policy, "Correct type");
      Assert.AreEqual("Test!", ((TestNamingPolicy) policy).TestString, "String value correct");
    }
  }
}

