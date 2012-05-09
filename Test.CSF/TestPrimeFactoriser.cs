using System;
using NUnit.Framework;
using CSF;
using System.Linq;
using System.Collections.Generic;

namespace Test.CSF
{
  [TestFixture]
  public class TestPrimeFactoriser
  {
    #region tests
    
    [Test]
    public void TestGenerate()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results  = generator.GeneratePrimes(15).ToList();
      
      Assert.AreEqual(6, results.Count, "Correct count");
      Assert.IsTrue(results.Contains(2), "Contains 2");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(13), "Contains 13");
    }
    
    [Test]
    public void TestGenerateRepeatedly()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results  = generator.GeneratePrimes(15).ToList();
      
      Assert.IsTrue(results.Contains(2), "Contains 2");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(13), "Contains 13");
      Assert.AreEqual(6, results.Count, "Correct count");
      
      results  = generator.GeneratePrimes(15).ToList();
      
      Assert.IsTrue(results.Contains(2), "Still contains 2");
      Assert.IsTrue(results.Contains(3), "Still contains 3");
      Assert.IsTrue(results.Contains(5), "Still contains 5");
      Assert.IsTrue(results.Contains(7), "Still contains 7");
      Assert.IsTrue(results.Contains(11), "Still contains 11");
      Assert.IsTrue(results.Contains(13), "Still contains 13");
      Assert.AreEqual(6, results.Count, "Still correct count");
    }
    
    [Test]
    [Explicit("This test takes a long time to run, depending on your CPU.  An Intel i5 can run this in about 40 seconds")]
    public void TestGenerateLots()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results  = generator.GeneratePrimes(1299709).ToList();
      Assert.AreEqual(100000, results.Count, "Correct count");
    }
    
    [Test]
    public void TestGetPrimeFactors()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results = generator.GetPrimeFactors(5775).ToList();
      
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.AreEqual(5, results.Count, "Correct count");
    }
    
    [Test]
    public void TestGetCommonPrimeFactors()
    {
      PrimeFactoriser generator = PrimeFactoriser.Default;
      IList<int> results = generator.GetCommonPrimeFactors(5775, 23100).ToList();
      
      Assert.IsTrue(results.Contains(5), "Contains 5");
      Assert.IsTrue(results.Contains(3), "Contains 3");
      Assert.IsTrue(results.Contains(11), "Contains 11");
      Assert.IsTrue(results.Contains(7), "Contains 7");
      Assert.AreEqual(5, results.Count, "Correct count");
    }
    
    #endregion
  }
}

