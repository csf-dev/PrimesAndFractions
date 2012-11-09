using System;
using NUnit.Framework;
using Moq;
using CSF.Patterns.ServiceLayer;
using System.Collections.Generic;

namespace Test.CSF.Patterns.ServiceLayer
{
  [TestFixture]
  public class TestRequestDispatcherBase
  {
    #region fields

    private Mock<IRequestDispatcher> MockDispatcher;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      this.MockDispatcher = new Mock<IRequestDispatcher>();
    }

    #endregion

    #region tests

    [Test]
    public void TestRegister()
    {
      IRequestDispatcher dispatcher = new DummyRequestDispatcher(this.MockDispatcher.Object);

      this.MockDispatcher
        .Setup(x => x.Register(It.IsAny<Type>(), It.IsAny<IRequestHandler>()))
        .Returns(this.MockDispatcher.Object);

      dispatcher.Register<MockRequestOne,MockRequestHandlerOne>();

      this.MockDispatcher
        .Verify(x => x.Register(typeof(MockRequestOne), It.IsAny<MockRequestHandlerOne>()),
                Times.Once());
    }

    [Test]
    public void TestRegisterFromAssemblyOf()
    {
      IRequestDispatcher dispatcher = new DummyRequestDispatcher(this.MockDispatcher.Object);

      this.MockDispatcher
        .Setup(x => x.Register(It.IsAny<Type>(), It.IsAny<IRequestHandler>()))
        .Returns(this.MockDispatcher.Object);

      dispatcher.RegisterFromAssemblyOf<DummyRequestDispatcher>();

      this.MockDispatcher
        .Verify(x => x.Register(typeof(MockRequestOne), It.IsAny<MockRequestHandlerOne>()),
                Times.Once());

      this.MockDispatcher
        .Verify(x => x.Register(typeof(MockRequestTwo), It.IsAny<MockRequestHandlerTwo>()),
                Times.Once());

      this.MockDispatcher
        .Verify(x => x.Register(typeof(MockRequestThree), It.IsAny<MockAbstractRequestHandler>()),
                Times.Never());

      this.MockDispatcher
        .Verify(x => x.Register(It.IsAny<Type>(), It.IsAny<IRequestHandler>()),
                Times.Exactly(2));
    }

    #endregion

    #region contained type (stub implementation of the base class we are testing, hands off to a mock)

    public class DummyRequestDispatcher : RequestDispatcherBase
    {
      private IRequestDispatcher Implementation;

      public override IResponse Dispatch (IRequest request)
      {
        return this.Implementation.Dispatch(request);
      }

      public override void DispatchRequestOnly (IRequest request)
      {
        this.Implementation.DispatchRequestOnly(request);
      }

      public override bool CanDispatch (Type requestType)
      {
        return this.Implementation.CanDispatch(requestType);
      }

      public override IDictionary<Type, IRequestHandler> GetRegisteredHandlers ()
      {
        return this.Implementation.GetRegisteredHandlers();
      }

      public override IRequestDispatcher Register (Type requestType, IRequestHandler handler)
      {
        this.Implementation.Register(requestType, handler);
        return this;
      }

      public override IRequestDispatcher Unregister (Type requestType)
      {
        this.Implementation.Unregister(requestType);
        return this;
      }

      public DummyRequestDispatcher(IRequestDispatcher implementation)
      {
        if(implementation == null)
        {
          throw new ArgumentNullException("implementation");
        }

        this.Implementation = implementation;
      }
    }

    #endregion
  }
}

