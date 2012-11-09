using System;
using NUnit.Framework;
using CSF.Patterns.IoC;

namespace Test.CSF.Patterns.IoC
{
  [TestFixture]
  public class TestServiceLocator
  {
    #region fields

    private MockServiceOne CachedService;

    #endregion

    #region tests
    
    [Test]
    [Category("Integration")]
    public void TestUsage()
    {
      ServiceLocator.Select(x => {
        x.Select<IMockService>(() => new MockServiceOne());
      });
      
      IMockService service = ServiceLocator.Get<IMockService>();
      
      Assert.IsInstanceOfType(typeof(MockServiceOne), service, "Correct type");
      Assert.AreEqual(typeof(MockServiceOne).Name, service.GetString(), "Correct output");
      
      IMockService secondService = ServiceLocator.Get<IMockService>();
      
      Assert.AreSame(service, secondService, "Same cached instance returned");
      
      ServiceLocator.Select(x => {
        x.Select<IMockService>(() => new MockServiceTwo());
      });

      service = ServiceLocator.Get<IMockService>();
      
      Assert.AreNotSame(service, secondService, "Services are no longer the same");
      Assert.IsInstanceOfType(typeof(MockServiceTwo), service, "Second service correct type");
      Assert.AreEqual(typeof(MockServiceTwo).Name, service.GetString(), "Second service correct output");
    }

    
    [Test]
    [Category("Integration")]
    public void TestDisposedAfterReplacement()
    {
      ServiceLocator.Select(x => {
        x.Select<IMockService>(() => {
          this.CachedService = new MockServiceOne();
          return this.CachedService;
        });
      });

      IMockService service = ServiceLocator.Get<IMockService>();

      Assert.AreEqual(this.CachedService, service, "Returns cached service");
      Assert.IsFalse(this.CachedService.IsDisposed, "Not disposed yet");

      ServiceLocator.Select(x => {
        x.Select<IMockService>(() => new MockServiceTwo());
      });

      service = ServiceLocator.Get<IMockService>();

      Assert.AreNotEqual(this.CachedService, service, "Service replaced correctly");
      Assert.IsTrue(this.CachedService.IsDisposed, "Disposed");
    }
    
    #endregion
    
    #region mocks
    
    public interface IMockService
    {
      string GetString();
    }
    
    public class MockServiceOne : IMockService, IDisposable
    {
      public bool IsDisposed = false;

      public string GetString()
      {
        return this.GetType().Name;
      }

      public void Dispose()
      {
        this.IsDisposed = true;
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

