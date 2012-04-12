using System;
using NUnit.Framework;
using CSF.IO;
using System.Collections.Generic;

namespace Test.CSF.IO
{
  [TestFixture]
  public class TestTabularDataParser
  {
    #region CSV tests
    
    [Test]
    public void TestReadCsv()
    {
      string input = "foo,bar,baz\r\n" +
                     "wibble,wobble,spong";
      IList<IList<string>> output;
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Csv);
      
      output = parser.Read(input);
      Assert.AreEqual(2, output.Count, "Correct row count");
      Assert.AreEqual("wibble", output[1][0], "Correct data in second row, first column");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
    }
    
    [Test]
    public void TestReadCsvWithQuotes()
    {
      string input = "foo,bar,baz\r\n" +
                     "wibble,    wobble   ,spong\r\n" +
                     "\"  foo\",\"\"\"bar\"\"\",\"A big, \"\"big, test!\"";
      IList<IList<string>> output;
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Csv);
      
      output = parser.Read(input);
      Assert.AreEqual(3, output.Count, "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("  foo", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"bar\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("A big, \"big, test!", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestReadCsvWithQuotedUnicode()
    {
      string input = "foo,bar,baz\r\n" +
                     "wibble,    wobble   ,spong\r\n" +
                     "\"  foo\",\"\"\"bar\"\"\",\"A big, \"\"big, ¥en test!\"";
      IList<IList<string>> output;
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Csv);
      
      output = parser.Read(input);
      Assert.AreEqual(3, output.Count, "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("  foo", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"bar\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("A big, \"big, ¥en test!", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestWriteCsv()
    {
      string output;
      string expectedOutput = "foo,bar,baz\r\n" +
                              "wibble,wobble,spong";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",    "baz"   },
                                                    new string[] { "wibble", "wobble", "spong" } };
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Csv);
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    [Test]
    public void TestWriteCsvWithQuotes()
    {
      string output;
      string expectedOutput = "foo,bar,baz\r\n" +
                              "wibble,wobble,spong\r\n" +
                              "\"  foo\",\"\"\"bar\"\"\",\"baz,bork\"";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",     "baz"      },
                                                    new string[] { "wibble", "wobble",  "spong"    },
                                                    new string[] { "  foo",  "\"bar\"", "baz,bork" }};
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Csv);
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    #endregion
    
    #region TSV tests
    
    [Test]
    public void TestReadTsv()
    {
      string input = "foo\tbar\tbaz\n" +
                     "wibble\twobble\tspong";
      IList<IList<string>> output;
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Tsv);
      
      output = parser.Read(input);
      Assert.AreEqual(2, output.Count, "Correct row count");
      Assert.AreEqual("wibble", output[1][0], "Correct data in second row, first column");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
    }
    
    [Test]
    public void TestReadTsvWithQuotes()
    {
      string input = "foo\tbar\tbaz\n" +
                     "wibble\t    wobble   \tspong\n" +
                     "\"  foo\"\t\"\"\"bar\"\"\"\t\"A big, \"\"big, test!\"";
      IList<IList<string>> output;
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Tsv);
      
      output = parser.Read(input);
      Assert.AreEqual(3, output.Count, "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("\"  foo\"", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"\"\"bar\"\"\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("\"A big, \"\"big, test!\"", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestReadTsvWithQuotedUnicode()
    {
      string input = "foo\tbar\tbaz\n" +
                     "wibble\t    wobble   \tspong\n" +
                     "\"  foo\"\t\"\"\"bar\"\"\"\t\"A big, \"\"big, ¥en test!\"";
      IList<IList<string>> output;
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Tsv);
      
      output = parser.Read(input);
      Assert.AreEqual(3, output.Count, "Correct row count");
      Assert.AreEqual("wobble", output[1][1], "Correct data in second row, second column");
      Assert.AreEqual("\"  foo\"", output[2][0], "Correct data in third row, first column");
      Assert.AreEqual("\"\"\"bar\"\"\"", output[2][1], "Correct data in third row, second column");
      Assert.AreEqual("\"A big, \"\"big, ¥en test!\"", output[2][2], "Correct data in third row, third column");
    }
    
    [Test]
    public void TestWriteTsv()
    {
      string output;
      string expectedOutput = "foo\tbar\tbaz\n" +
                              "wibble\twobble\tspong";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",    "baz"   },
                                                    new string[] { "wibble", "wobble", "spong" } };
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Tsv);
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    [Test]
    public void TestWriteTsvWithQuotes()
    {
      string output;
      string expectedOutput = "foo\tbar\tbaz\n" +
                              "wibble\t  wobble  \tspong\n" +
                              "  foo\t\"bar\"\tbaz,bork";
      IList<IList<string>> input = new string[][] { new string[] { "foo",    "bar",     "baz"      },
                                                    new string[] { "wibble", "  wobble  ",  "spong"    },
                                                    new string[] { "  foo",  "\"bar\"", "baz,bork" }};
      ITabularDataParser parser = new TabularDataParser(TabularDataFormat.Tsv);
      
      output = parser.Write(input);
      Assert.AreEqual(expectedOutput, output, "Correct string rendering");
    }
    
    #endregion
  }
}

