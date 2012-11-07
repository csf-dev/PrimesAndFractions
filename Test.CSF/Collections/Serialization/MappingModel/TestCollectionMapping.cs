using System;
using NUnit.Framework;

namespace Test.CSF.Collections.Serialization.MappingModel
{
  [TestFixture]
  public class TestCollectionMapping
  {
    #region validation tests

    [Test]
    [Ignore("This test is not written yet")]
    public void TestValidateNoMapAs()
    {

    }

    [Test]
    [Ignore("This test is not written yet")]
    public void TestValidateAggregateMissing()
    {

    }

    [Test]
    [Ignore("This test is not written yet")]
    public void TestValidateAggregateWrongType()
    {

    }

    #endregion

    #region serialization testing

    [Test]
    [Ignore("This test is a placeholder for multiple tests that need writing")]
    public void TestSerialize()
    {
      /* There are multiple tests to write here:
       * 
       * * Successful serialization using separate keys
       * * Successful serialization using an aggregate key
       * * Writing a flag value using separate keys
       * * Writing a flag value using an aggregate key
       * * Mandatory failure to serialize child mapping using separate keys
       * * Mandatory failure to serialize child mapping using an aggregate key
       * * Failure to serialize because of a null value (should skip serialization of this object)
       */
    }

    [Test]
    [Ignore("This test is a placeholder for multiple tests that need writing")]
    public void TestDeserialize()
    {
      /* There are multiple tests to write here:
       * 
       * * Successful deserialization using separate keys
       * * Successful deserialization using an aggregate key
       * * Failing a flag value using separate keys
       * * Failing a flag value using an aggregate key
       * * Success with a flag value using separate keys
       * * Success with a flag value using an aggregate key
       * * Mandatory failure to deserialize child mapping using separate keys
       * * Mandatory failure to deserialize child mapping using an aggregate key
       * * Mandatory failure to deserialize this instance
       * * Failure to deserialize any items (should emit an empty collection)
       */
    }

    #endregion
  }
}

