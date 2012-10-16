using System;
using CSF.Patterns.ServiceLayer;

namespace Test.CSF.Patterns.ServiceLayer
{
  #region requests

  public class MockRequestOne : IRequest {}

  public class MockRequestTwo : IRequest {}

  public class MockRequestThree : IRequest {}

  #endregion

  #region responses

  public class MockResponse : Response
  {
    public IRequest Request;
  }

  #endregion

  #region handlers

  public class MockRequestHandlerOne : RequestHandlerBase<MockRequestOne,MockResponse>
  {
    public override MockResponse Handle (MockRequestOne request)
    {
      return new MockResponse() { Request = request };
    }
  }

  public class MockRequestHandlerTwo : RequestHandlerBase<MockRequestTwo,MockResponse>
  {
    public override MockResponse Handle (MockRequestTwo request)
    {
      return new MockResponse() { Request = request };
    }
  }

  public abstract class MockAbstractRequestHandler : RequestHandlerBase<MockRequestThree,MockResponse>
  {
    public override MockResponse Handle (MockRequestThree request)
    {
      return new MockResponse() { Request = request };
    }
  }

  #endregion
}

