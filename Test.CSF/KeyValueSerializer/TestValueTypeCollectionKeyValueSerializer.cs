using System;
using NUnit.Framework;
using CSF.KeyValueSerializer;
using System.Collections.Generic;
using System.Linq;

namespace Test.CSF.KeyValueSerializer
{
  [TestFixture]
  public class TestValueTypeCollectionKeyValueSerializer
  {
    [Test]
    public void TestDeserialize()
    {
      var serializer = new ValueTypeCollectionKeyValueSerializer<DateTime>();

      serializer.Map(x => {
        x.Composite()
          .Component("Year", m => m.Serialize(data => data.Year.ToString()))
          .Component("Month", m => m.Serialize(data => data.Month.ToString()))
          .Component("Day", m => m.Serialize(data => data.Day.ToString()))
          .Deserialize(data => {
            return new DateTime(Int32.Parse(data["Year"]), Int32.Parse(data["Month"]), Int32.Parse(data["Day"]));
          });
      });

      var collection = new Dictionary<string,string>();

      collection.Add("[0]Year",  "2010");
      collection.Add("[0]Month", "1");
      collection.Add("[0]Day",   "1");
      collection.Add("[1]Year",  "2011");
      collection.Add("[1]Month", "2");
      collection.Add("[1]Day",   "2");
      collection.Add("[2]Year",  "2012");
      collection.Add("[2]Month", "3");
      collection.Add("[2]Day",   "3");

      ICollection<DateTime> result = serializer.Deserialize(collection);

      Assert.IsNotNull(result, "Result not null");
      Assert.AreEqual(3, result.Count, "Result correct count");
      Assert.IsTrue(result.Any(x => x == new DateTime(2010, 1, 1)), "First item");
      Assert.IsTrue(result.Any(x => x == new DateTime(2011, 2, 2)), "Second item");
      Assert.IsTrue(result.Any(x => x == new DateTime(2012, 3, 3)), "Third item");
    }
  }
}

