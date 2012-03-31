using System;
using NUnit.Framework;
using CSF.IO;
using System.Collections.Generic;

namespace Test.CSF.IO
{
  [TestFixture]
  public class TestCsvDataParser
  {
    [Test]
    public void TestRead()
    {
      string input = @"foo,bar,baz
wibble,wobble,spong";
      IList<IList<string>> output;
      ITabularDataParser parser = new CsvDataParser();
      
      output = parser.Read(input);
      Assert.AreEqual(2, output.Count, "Correct row count");
      Assert.AreEqual("wibble", output[1][0], "Correct data in second row, first column");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
    }
    
    [Test]
    public void TestReadWithQuotes()
    {
      string input = @"foo,bar,baz
wibble,    wobble   ,spong
""  foo"",""""""bar"""""",""A big, """"big, test!""";
      IList<IList<string>> output;
      ITabularDataParser parser = new CsvDataParser();
      
      output = parser.Read(input);
      Assert.AreEqual(3, output.Count, "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("  foo", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"bar\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("A big, \"big, test!", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestReadWithQuotedUnicode()
    {
      string input = @"foo,bar,baz
wibble,    wobble   ,spong
""  foo"",""""""bar"""""",""A big, """"big, ¥en test!""";
      IList<IList<string>> output;
      ITabularDataParser parser = new CsvDataParser();
      
      output = parser.Read(input);
      Assert.AreEqual(3, output.Count, "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("  foo", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"bar\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("A big, \"big, ¥en test!", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestWrite()
    {
      string output;
      string expectedOutput = @"foo,bar,baz
wibble,wobble,spong";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",    "baz"   },
                                                    new string[] { "wibble", "wobble", "spong" } };
      ITabularDataParser parser = new CsvDataParser();
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    [Test]
    public void TestWriteWithQuotes()
    {
      string output;
      string expectedOutput = @"foo,bar,baz
wibble,wobble,spong
""  foo"",""""""bar"""""",""baz,bork""";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",     "baz"      },
                                                    new string[] { "wibble", "wobble",  "spong"    },
                                                    new string[] { "  foo",  "\"bar\"", "baz,bork" }};
      ITabularDataParser parser = new CsvDataParser();
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
  }
}

