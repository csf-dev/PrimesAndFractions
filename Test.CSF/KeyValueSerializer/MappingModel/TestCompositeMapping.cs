using System;
using NUnit.Framework;
using Moq;
using CSF.KeyValueSerializer.MappingModel;
using CSF.Reflection;
using System.Collections.Generic;

namespace Test.CSF.KeyValueSerializer.MappingModel
{
  [TestFixture]
  public class TestCompositeMapping
  {
    #region general tests

    [Test]
    public void TestGetKeyName()
    {
      var componentMap = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();

      componentMap.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", componentMap.Object);

      Assert.AreEqual("FooYear", mapping.GetKeyName("Year", new int[0]));
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(ArgumentException))]
    public void TestGetKeyNameMissingComponent()
    {
      var parent = new Mock<IMapping>();
      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.GetKeyName("Year", new int[0]);
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException),
                       ExpectedMessage = "A composite mapping must contain at least one component to be valid.")]
    public void TestValidateEnoughComponents()
    {
      var parent = new Mock<IMapping>();
      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Validate();
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException),
                       ExpectedMessage = "This composite mapping is 'useless'.  It must either expose a deserialization function or all of its components must expose serialization functions.")]
    public void TestValidateUseless()
    {
      var componentMap = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();

      componentMap.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", componentMap.Object);
      mapping.Validate();
    }

    #endregion

    #region serialization

    [Test]
    public void TestSerializeSuccess()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.SerializationFunction).Returns(d => d.Year.ToString());
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.SerializationFunction).Returns(d => d.Month.ToString());
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(date, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(2, result.Count, "Result count");
      Assert.IsTrue(result["FooYear"] == date.Year.ToString(), "First component");
      Assert.IsTrue(result["FooMonth"] == date.Month.ToString(), "Second component");
    }

    [Test]
    public void TestSerializeAndWriteFlag()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.SerializationFunction).Returns(d => d.Year.ToString());
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.SerializationFunction).Returns(d => d.Month.ToString());
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);
      mapping.FlagKey = "date";

      IDictionary<string,string> result;
      bool success = mapping.Serialize(date, out result, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.IsNotNull(result, "Result nullability");
      Assert.AreEqual(3, result.Count, "Result count");
      Assert.IsTrue(result["FooYear"] == date.Year.ToString(), "First component");
      Assert.IsTrue(result["FooMonth"] == date.Month.ToString(), "Second component");
      Assert.IsTrue(result["date"] == Boolean.TrueString, "Flag");
    }

    [Test]
    public void TestSerializeComponentFailure()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.SerializationFunction).Returns(d => d.Year.ToString());
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.SerializationFunction).Returns(d => null);
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(date, out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result nullability");
    }

    [Test]
    public void TestSerializeComponentException()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.SerializationFunction).Returns(d => d.Year.ToString());
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.SerializationFunction).Returns(d => {
        throw new Exception("Test exception");
      });
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);

      IDictionary<string,string> result;
      bool success = mapping.Serialize(date, out result, new int[0]);

      Assert.IsFalse(success, "Success");
      Assert.IsNull(result, "Result nullability");
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException))]
    public void TestSerializeNoComponents()
    {
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));

      IDictionary<string,string> result;
      mapping.Serialize(date, out result, new int[0]);
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidOperationException))]
    public void TestSerializeNoSerializationFunction()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.SerializationFunction).Returns(d => d.Year.ToString());
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.SerializationFunction).Returns((Func<DateTime,string>) null);
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);

      IDictionary<string,string> result;
      mapping.Serialize(date, out result, new int[0]);
    }

    #endregion

    #region deserialization

    [Test]
    public void TestDeserializeSuccess()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);
      mapping.DeserializationFunction = dict => new DateTime(Int32.Parse(dict["Year"]), Int32.Parse(dict["Month"]), 1);

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("FooYear", date.Year.ToString());
      data.Add("FooMonth", date.Month.ToString());

      DateTime output;
      bool success = mapping.Deserialize(data, out output, new int[0]);

      Assert.IsTrue(success, "Success");
      Assert.AreEqual(date.Year, output.Year, "Year");
      Assert.AreEqual(date.Month, output.Month, "Month");
    }

    [Test]
    public void TestDeserializeFlagFailure()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);
      mapping.DeserializationFunction = dict => new DateTime(Int32.Parse(dict["Year"]), Int32.Parse(dict["Month"]), 1);
      mapping.FlagKey = "flag";

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("FooYear", date.Year.ToString());
      data.Add("FooMonth", date.Month.ToString());

      DateTime output;
      bool success = mapping.Deserialize(data, out output, new int[0]);

      Assert.IsFalse(success, "Success");
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(InvalidMappingException))]
    public void TestDeserializeNoComponents()
    {
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.DeserializationFunction = dict => new DateTime(Int32.Parse(dict["Year"]), Int32.Parse(dict["Month"]), 1);

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("FooYear", date.Year.ToString());
      data.Add("FooMonth", date.Month.ToString());

      DateTime output;
      mapping.Deserialize(data, out output, new int[0]);
    }

    [Test]
    public void TestDeserializeAllComponentsMissing()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);
      mapping.DeserializationFunction = dict => new DateTime(Int32.Parse(dict["Year"]), Int32.Parse(dict["Month"]), 1);

      IDictionary<string,string> data = new Dictionary<string, string>();

      DateTime output;
      bool success = mapping.Deserialize(data, out output, new int[0]);

      Assert.IsFalse(success, "Success");
    }

    [Test]
    public void TestDeserializeSomeComponentsMissing()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);
      mapping.DeserializationFunction = dict => new DateTime(Int32.Parse(dict["Year"]), Int32.Parse(dict["Month"]), 1);

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("FooYear", date.Year.ToString());

      DateTime output;
      bool success = mapping.Deserialize(data, out output, new int[0]);

      Assert.IsFalse(success, "Success");
    }

    [Test]
    public void TestDeseralizeException()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);
      mapping.DeserializationFunction = dict => {
        throw new Exception("This is a test exception");
      };

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("FooYear", date.Year.ToString());
      data.Add("FooMonth", date.Month.ToString());

      DateTime output;
      bool success = mapping.Deserialize(data, out output, new int[0]);

      Assert.IsFalse(success, "Success");
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(MandatorySerializationException))]
    public void TestDeserializeMandatoryFailure()
    {
      var yearComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var monthComponent = new Mock<ICompositeComponentMapping<DateTime>>();
      var parent = new Mock<IMapping>();
      DateTime date = DateTime.Today;

      yearComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooYear");
      yearComponent.SetupGet(x => x.ComponentIdentifier).Returns("Year");
      monthComponent.Setup(x => x.GetKeyName(It.IsAny<int[]>())).Returns("FooMonth");
      monthComponent.SetupGet(x => x.ComponentIdentifier).Returns("Month");

      CompositeMapping<DateTime> mapping = new CompositeMapping<DateTime>(parent.Object,
                                                                          Reflect.Property<Foo>(x => x.TestDateTime));
      mapping.Components.Add("Year", yearComponent.Object);
      mapping.Components.Add("Month", monthComponent.Object);
      mapping.DeserializationFunction = dict => new DateTime(Int32.Parse(dict["Year"]), Int32.Parse(dict["Month"]), 1);
      mapping.Mandatory = true;

      IDictionary<string,string> data = new Dictionary<string, string>();
      data.Add("FooYear", date.Year.ToString());

      DateTime output;
      mapping.Deserialize(data, out output, new int[0]);
    }

    #endregion
  }
}

