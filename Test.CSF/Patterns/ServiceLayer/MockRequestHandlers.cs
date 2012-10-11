using System;
using CSF.Patterns.ServiceLayer;

namespace Test.CSF.Patterns.ServiceLayer
{
  public class MockRequestHandlerOne : RequestHandlerBase<MockRequestOne,MockResponse>
  {
    public override MockResponse Handle (MockRequestOne request)
    {
      return new MockResponse() { Request = request };
    }
  }

  public class MockRequestOne : IRequest {}

  public class MockRequestHandlerTwo : RequestHandlerBase<MockRequestTwo,MockResponse>
  {
    public override MockResponse Handle (MockRequestTwo request)
    {
      return new MockResponse() { Request = request };
    }
  }

  public class MockRequestTwo : IRequest {}

  public class MockResponse : Response
  {
    public IRequest Request;
  }
}

