using System;
using NUnit.Framework;
using CSF.Patterns.ServiceLayer;

namespace Test.CSF.Patterns.ServiceLayer
{
  [TestFixture]
  public class TestLocalRequestDispatcher
  {
    [Test]
    public void TestRegister()
    {
      LocalRequestDispatcher dispatcher = new LocalRequestDispatcher();

      dispatcher
        .Register<MockRequestOne, MockRequestHandlerOne>()
        .Register<MockRequestTwo, MockRequestHandlerTwo>();

      var handlers = dispatcher.GetRegisteredHandlers();

      Assert.AreEqual(2, handlers.Count);
      Assert.IsTrue(handlers.ContainsKey(typeof(MockRequestOne)), "Contains a definition for request 1");
      Assert.IsTrue(handlers.ContainsKey(typeof(MockRequestTwo)), "Contains a definition for request 2");

      Assert.IsInstanceOfType(typeof(MockRequestHandlerOne), handlers[typeof(MockRequestOne)], "Correct handler 1");
      Assert.IsInstanceOfType(typeof(MockRequestHandlerTwo), handlers[typeof(MockRequestTwo)], "Correct handler 2");
    }

    [Test]
    public void TestDispatch()
    {
      LocalRequestDispatcher dispatcher = new LocalRequestDispatcher();

      dispatcher
        .Register<MockRequestOne, MockRequestHandlerOne>()
        .Register<MockRequestTwo, MockRequestHandlerTwo>();

      MockRequestOne request = new MockRequestOne();
      MockResponse response = dispatcher.Dispatch<MockResponse>(request);

      Assert.AreSame(request, response.Request);
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(RequestDispatchException),
                       ExpectedMessage = "Could not dispatch the request as there is no registered handler. " +
                                         "Request type: `Test.CSF.Patterns.ServiceLayer.MockRequestOne'")]
    public void TestDispatchNotRegistered()
    {
      LocalRequestDispatcher dispatcher = new LocalRequestDispatcher();

      dispatcher
        .Register<MockRequestTwo, MockRequestHandlerTwo>();

      dispatcher.Dispatch(new MockRequestOne());
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(RequestDispatchException),
                       ExpectedMessage = "Could not dispatch the request; it is null.")]
    public void TestDispatchNullRequest()
    {
      LocalRequestDispatcher dispatcher = new LocalRequestDispatcher();

      dispatcher
        .Register<MockRequestOne, MockRequestHandlerOne>()
        .Register<MockRequestTwo, MockRequestHandlerTwo>();

      dispatcher.Dispatch(null);
    }
  }
}

