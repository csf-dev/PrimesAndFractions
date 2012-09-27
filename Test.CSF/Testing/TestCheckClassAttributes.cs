using System;
using CSF.Testing.Reflection;
using CSF.Testing.Mocks;
using CSF.Testing.Mocks.NonSerializableNamespace;
using NUnit.Framework;

namespace Test.CSF.Testing
{
  [TestFixture]
  public class TestCheckClassAttributes
  {
    [Test]
    public void TestExecuteChecks()
    {
      CheckClassAttributes<SerializableAttribute>.FromAssemblyOfType<SampleClass>()
        .Except<NonSerializableClass>()
        .ExceptNamespace<SampleClass1>()
        .ExecuteChecks();
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(AssertionException),
                       ExpectedMessage = @"Failed checks for SerializableAttribute, types that failed are:
CSF.Testing.Mocks.NonSerializableClass")]
    public void TestExecuteChecksFailure()
    {
      CheckClassAttributes<SerializableAttribute>.FromAssemblyOfType<SampleClass>()
        .ExceptNamespace<SampleClass1>()
        .ExecuteChecks();
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(AssertionException),
                       ExpectedMessage = @"Failed checks for SerializableAttribute, types that failed are:
CSF.Testing.Mocks.NonSerializableNamespace.SampleClass1
CSF.Testing.Mocks.NonSerializableNamespace.SampleClass2")]
    public void TestExecuteChecksFailureNamespace()
    {
      CheckClassAttributes<SerializableAttribute>.FromAssemblyOfType<SampleClass>()
        .Except<NonSerializableClass>()
        .ExecuteChecks();
    }
  }
}

