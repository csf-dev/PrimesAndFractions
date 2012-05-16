using System;
using NUnit.Framework;
using CSF.Patterns.IoC;

namespace Test.CSF.Patterns.IoC
{
  [TestFixture]
  public class TestServiceLocator
  {
    #region tests
    
    [Test]
    [Category("Integration")]
    public void TestUsage()
    {
      ServiceLocator.Select<IMockService>(() => { return new MockServiceOne(); });
      
      IMockService service = ServiceLocator.Get<IMockService>();
      
      Assert.IsInstanceOfType(typeof(MockServiceOne), service, "Correct type");
      Assert.AreEqual(typeof(MockServiceOne).Name, service.GetString(), "Correct output");
      
      IMockService secondService = ServiceLocator.Get<IMockService>();
      
      Assert.AreSame(service, secondService, "Same cached instance returned");
      
      ServiceLocator.Select<IMockService>(() => {return new MockServiceTwo(); });
      
      service = ServiceLocator.Get<IMockService>();
      
      Assert.AreNotSame(service, secondService, "Services are no longer the same");
      Assert.IsInstanceOfType(typeof(MockServiceTwo), service, "Second service correct type");
      Assert.AreEqual(typeof(MockServiceTwo).Name, service.GetString(), "Second service correct output");
    }
    
    #endregion
    
    #region mocks
    
    public interface IMockService
    {
      string GetString();
    }
    
    public class MockServiceOne : IMockService
    {
      public string GetString()
      {
        return this.GetType().Name;
      }
    }
    
    public class MockServiceTwo : IMockService
    {
      public string GetString()
      {
        return this.GetType().Name;
      }
    }
    
    #endregion
  }
}

