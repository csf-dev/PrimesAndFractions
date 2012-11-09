using System;
using NUnit.Framework;
using CSF.KeyValueSerializer.MappingModel;
using System.Collections.Generic;

namespace Test.CSF.KeyValueSerializer.MappingModel
{
  [TestFixture]
  public class TestMappingBase
  {
    #region tests

    [Test]
    public void TestAttachKeyNamingPolicy()
    {
      StubMapping mapping = new StubMapping();

      mapping.AttachKeyNamingPolicy<TestNamingPolicy>();

      Assert.IsNotNull(mapping.KeyNamingPolicy);
      Assert.IsInstanceOfType(typeof(TestNamingPolicy), mapping.KeyNamingPolicy);
      Assert.AreSame(mapping, mapping.KeyNamingPolicy.AssociatedMapping);
    }

    [Test]
    public void TestAttachKeyNamingPolicyWithFactoryMethod()
    {
      StubMapping mapping = new StubMapping();

      mapping.AttachKeyNamingPolicy<TestNamingPolicy>(map => new TestNamingPolicy(map) { TestString = "Test!" });

      Assert.IsNotNull(mapping.KeyNamingPolicy);
      Assert.IsInstanceOfType(typeof(TestNamingPolicy), mapping.KeyNamingPolicy);
      Assert.AreSame(mapping, mapping.KeyNamingPolicy.AssociatedMapping);
      Assert.AreEqual("Test!", ((TestNamingPolicy) mapping.KeyNamingPolicy).TestString);
    }

    [Test]
    public void TestCreateKeyNamingPolicy()
    {
      StubMapping mapping = new StubMapping();

      TestNamingPolicy policy = mapping.CreateKeyNamingPolicy<TestNamingPolicy>(mapping);

      Assert.IsNotNull(policy);
      Assert.AreSame(mapping, policy.AssociatedMapping);
    }

    [Test]
    public void TestSatisfiesFlag()
    {
      StubMapping mapping = new StubMapping();
      IDictionary<string,string> values = new Dictionary<string, string>();

      Assert.IsTrue(mapping.SatisfiesFlag(values), "Satisfies with no requirement");

      mapping.FlagKey = "foo";
      Assert.IsFalse(mapping.SatisfiesFlag(values), "Does not satisfy - key not present");

      values.Clear();
      values.Add("Foo", "bar");
      Assert.IsFalse(mapping.SatisfiesFlag(values), "Does not satisfy - key not present (case sensitive)");

      values.Clear();
      values.Add("foo", String.Empty);
      Assert.IsFalse(mapping.SatisfiesFlag(values), "Does not satisfy - empty value");

      values.Clear();
      values.Add("foo", "bar");
      Assert.IsTrue(mapping.SatisfiesFlag(values), "Satisfies with any value");

      mapping.FlagValue = "bar";
      values.Clear();
      values.Add("foo", "bar");
      Assert.IsTrue(mapping.SatisfiesFlag(values), "Satisfies with correct value");

      values.Clear();
      values.Add("foo", "baz");
      Assert.IsFalse(mapping.SatisfiesFlag(values), "Does not satisfy - wrong value");

      values.Clear();
      values.Add("foo", String.Empty);
      Assert.IsFalse(mapping.SatisfiesFlag(values), "Does not satisfy - empty value (again)");
    }

    [Test]
    public void TestWriteFlag()
    {
      StubMapping mapping = new StubMapping();

      IDictionary<string,string> values = new Dictionary<string, string>();

      values.Clear();
      mapping.WriteFlag(values);
      Assert.AreEqual(0, values.Count, "Nothing written (no flag)");

      values.Clear();
      mapping.FlagKey = String.Empty;
      mapping.WriteFlag(values);
      Assert.AreEqual(0, values.Count, "Nothing written (empty flag)");

      values.Clear();
      mapping.FlagKey = "foo";
      mapping.WriteFlag(values);
      Assert.AreEqual(1, values.Count, "Flag written (no value)");
      Assert.AreEqual(Boolean.TrueString, values["foo"], "Correct flag written (no value)");

      values.Clear();
      mapping.FlagKey = "foo";
      mapping.FlagValue = String.Empty;
      mapping.WriteFlag(values);
      Assert.AreEqual(1, values.Count, "Flag written (empty value)");
      Assert.AreEqual(Boolean.TrueString, values["foo"], "Correct flag written (empty value)");

      values.Clear();
      mapping.FlagKey = "foo";
      mapping.FlagValue = "bar";
      mapping.WriteFlag(values);
      Assert.AreEqual(1, values.Count, "Flag written (with value)");
      Assert.AreEqual("bar", values["foo"], "Correct flag written (with value)");
    }

    #endregion

    #region contained type

    class StubMapping : MappingBase<int>
    {
      #region implemented abstract members of CSF.KeyValueSerializer.MappingModel.MappingBase

      public override bool Serialize (int data, out IDictionary<string, string> result, int[] collectionIndices)
      {
        throw new System.NotImplementedException ();
      }

      public override bool Deserialize (IDictionary<string, string> data, out int result, int[] collectionIndices)
      {
        throw new System.NotImplementedException ();
      }

      #endregion

      #region constructor

      public StubMapping() : base(null, null, true) {}

      #endregion
    }

    #endregion
  }
}

