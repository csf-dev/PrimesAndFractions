//
// IManipulatesFilenameExtensions.cs
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

namespace CSF.IO
{
  /// <summary>
  /// A service which is able to manipulate the extensions of filenames.
  /// </summary>
  public interface IManipulatesFilenameExtensions
  {
    /// <summary>
    /// Given a filename which might have zero or more extensions, gets the last extension.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If the file has only one extension then this returns it.  If it has multiple extensions then only the last one
    /// is returned.  If the file has no extensions then a <c>null</c> reference is returned.
    /// </para>
    /// </remarks>
    /// <returns>The last extension.</returns>
    /// <param name="filename">Filename.</param>
    string GetLastExtension(string filename);

    /// <summary>
    /// Given a filename which might have zero or more extensions, gets a collection of those extensions.
    /// </summary>
    /// <returns>The extensions.</returns>
    /// <param name="filename">Filename.</param>
    string[] GetExtensions(string filename);

    /// <summary>
    /// Gets a copy of the given filename, except with the last extension removed.
    /// </summary>
    /// <returns>The filename, with the last extension removed.</returns>
    /// <param name="filename">Filename.</param>
    string RemoveLastExtension(string filename);

    /// <summary>
    /// Gets a copy of the given filename, except with the first extension removed.
    /// </summary>
    /// <returns>The filename, with the first extension removed.</returns>
    /// <param name="filename">Filename.</param>
    string RemoveFirstExtension(string filename);

    /// <summary>
    /// Gets a copy of the given filename, except with one of the extensions removed.
    /// </summary>
    /// <returns>The filename, with the specified extension removed.</returns>
    /// <param name="filename">Filename.</param>
    /// <param name="index">The index of the extension to remove.</param>
    string RemoveExtension(string filename, int index);

    /// <summary>
    /// Gets a copy of the given filename, except with the last extension replaced with the specified replacement.
    /// </summary>
    /// <returns>The filename, with the last extension replaced.</returns>
    /// <param name="filename">Filename.</param>
    /// <param name="newExtension">The replacement extension.</param>
    string ReplaceLastExtension(string filename, string newExtension);

    /// <summary>
    /// Gets a copy of the given filename, except with the first extension replaced with the specified replacement.
    /// </summary>
    /// <returns>The filename, with the first extension replaced.</returns>
    /// <param name="filename">Filename.</param>
    /// <param name="newExtension">The replacement extension.</param>
    string ReplaceFirstExtension(string filename, string newExtension);

    /// <summary>
    /// Gets a copy of the given filename, except with one of the extensions replaced with the specified replacement.
    /// </summary>
    /// <returns>The filename, with an extension replaced.</returns>
    /// <param name="filename">Filename.</param>
    /// <param name="index">The index of the extension to replace.</param>
    /// <param name="newExtension">The replacement extension.</param>
    string ReplaceExtension(string filename, int index, string newExtension);

    /// <summary>
    /// Gets a copy of the given filename, with an additional extension added as the new last extension.
    /// </summary>
    /// <returns>The modified filename.</returns>
    /// <param name="filename">Filename.</param>
    /// <param name="extraExtension">The extension to add.</param>
    string AppendExtension(string filename, string extraExtension);

    /// <summary>
    /// Gets a copy of the given filename, with an additional extension added as the new first extension.
    /// </summary>
    /// <returns>The modified filename.</returns>
    /// <param name="filename">Filename.</param>
    /// <param name="extraExtension">The extension to add.</param>
    string PrependExtension(string filename, string extraExtension);

    /// <summary>
    /// Gets a copy of the given filename, with an additional extension added at a specified index.
    /// </summary>
    /// <returns>The modified filename.</returns>
    /// <param name="filename">Filename.</param>
    /// <param name="index">The index at which to insert the new extension.</param>
    /// <param name="extraExtension">The extension to add.</param>
    string InsertExtension(string filename, int index, string extraExtension);

    /// <summary>
    /// Gets a builder instance which may be used to perform advanced manipulation upon a filename.
    /// </summary>
    /// <returns>The builder.</returns>
    /// <param name="filename">Filename.</param>
    IBuildsFilenamesWithExtensions CreateBuilder(string filename);
  }
}
