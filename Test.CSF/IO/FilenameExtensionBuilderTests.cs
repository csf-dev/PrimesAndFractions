//
// FilenameExtensionBuilderTests.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2018 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using CSF.IO;
using NUnit.Framework;

namespace Test.CSF.IO
{
  [TestFixture]
  public class FilenameExtensionBuilderTests
  {
    [Test]
    public void BaseName_throws_ArgumentNullException_if_setter_is_invoked_with_null()
    {
      // Arrange
      var sut = new FilenameExtensionBuilder("foo", new [] { "bar" });

      // Act & assert
      Assert.That(() => sut.BaseName = null, Throws.InstanceOf<ArgumentNullException>());
    }

    [Test]
    public void BaseName_throws_ArgumentException_if_setter_is_invoked_with_empty_string()
    {
      // Arrange
      var sut = new FilenameExtensionBuilder("foo", new [] { "bar" });

      // Act & assert
      Assert.That(() => sut.BaseName = String.Empty, Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void ToString_combines_base_name_with_extensions_using_separator_character()
    {
      // Arrange
      var sut = new FilenameExtensionBuilder("foo", new [] { "bar", "baz" }, '-');

      // Act
      var result = sut.ToString();

      // Assert
      Assert.That(result, Is.EqualTo("foo-bar-baz"));
    }

    [Test]
    public void ToString_ignores_extensions_which_are_null()
    {
      // Arrange
      var sut = new FilenameExtensionBuilder("foo", new [] { "bar", null, "baz" }, '-');

      // Act
      var result = sut.ToString();

      // Assert
      Assert.That(result, Is.EqualTo("foo-bar-baz"));
    }

    [Test]
    public void ToString_does_not_ignore_extensions_which_are_empty_string()
    {
      // Arrange
      var sut = new FilenameExtensionBuilder("foo", new [] { "bar", String.Empty, "baz" }, '-');

      // Act
      var result = sut.ToString();

      // Assert
      Assert.That(result, Is.EqualTo("foo-bar--baz"));
    }

    [TestCase("foo.bar.baz")]
    [TestCase(".foo.bar.baz")]
    [TestCase("foo..bar.baz")]
    [TestCase("foo")]
    [TestCase(".foo")]
    [TestCase("foo.")]
    [TestCase("foo..")]
    [TestCase("foo.bar..")]
    public void Parse_and_ToString_can_roundtrip_valid_filenames(string filename)
    {
      // Arrange
      var sut = FilenameExtensionBuilder.Parse(filename);

      // Act
      var result = sut.ToString();

      // Assert
      Assert.That(result, Is.EqualTo(filename));
    }

    [Test]
    public void Parse_recognises_hidden_dot_files()
    {
      // Act
      var result = FilenameExtensionBuilder.Parse(".foo");

      // Assert
      Assert.That(result.BaseName, Is.EqualTo(".foo"));
    }

    [Test]
    public void Parse_recognises_hidden_dot_files_with_extension()
    {
      // Act
      var result = FilenameExtensionBuilder.Parse(".foo.bar");

      // Assert
      Assert.That(result.BaseName, Is.EqualTo(".foo"));
    }

    [Test]
    public void Parse_throws_argument_null_exception_for_null_filename()
    {
      // Act & assert
      Assert.That(() => FilenameExtensionBuilder.Parse(null), Throws.InstanceOf<ArgumentNullException>());
    }

    [Test]
    public void Parse_throws_argument_exception_for_empty_filename()
    {
      // Act & assert
      Assert.That(() => FilenameExtensionBuilder.Parse(String.Empty), Throws.InstanceOf<ArgumentException>());
    }

    [Test]
    public void Parse_throws_argument_exception_for_filename_which_is_a_single_dot()
    {
      // Act & assert
      Assert.That(() => FilenameExtensionBuilder.Parse("."), Throws.InstanceOf<ArgumentException>());
    }
  }
}
