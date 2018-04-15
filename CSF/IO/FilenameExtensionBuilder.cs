using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.IO
{
  /// <summary>
  /// A builder which may be used to perform advanced manipulations upon a filename and its extensions.
  /// </summary>
  public class FilenameExtensionBuilder : IBuildsFilenamesWithExtensions
  {
    const char DefaultExtensionSeparator = '.';

    readonly char extensionCharacter;
    string baseName;

    /// <summary>
    /// Gets or sets the base filename (without any extensions).
    /// </summary>
    /// <value>The filename.</value>
    public string BaseName
    {
      get { return baseName; }
      set {
        if(value == null)
          throw new ArgumentNullException(nameof(value));
        if(value.Length == 0)
          throw new ArgumentException(Resources.ExceptionMessages.FileBaseNameMustNotBeEmpty);

        baseName = value;
      }
    }

    /// <summary>
    /// Gets an ordered collection of the extensions.  This collection is mutable; <c>null</c> extensions are ignored.
    /// </summary>
    /// <value>The extensions.</value>
    public IList<string> Extensions { get; }

    /// <summary>
    /// Converts the state of the current instance into a <c>System.String</c>.
    /// </summary>
    /// <returns>A <see cref="String"/> that represents the current <see cref="IBuildsFilenamesWithExtensions"/>.</returns>
    public override string ToString()
    {
      var parts = new List<string>(Extensions);
      parts.Insert(0, BaseName);

      return String.Join(extensionCharacter.ToString(), parts.Where(x => x != null));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.IO.FilenameExtensionBuilder"/> class.
    /// </summary>
    /// <param name="baseName">Base name.</param>
    /// <param name="extensions">Extensions.</param>
    /// <param name="extensionCharacter">Extension character.</param>
    public FilenameExtensionBuilder(string baseName,
                                    IReadOnlyList<string> extensions,
                                    char extensionCharacter = DefaultExtensionSeparator)
    {
      if(extensions == null)
        throw new ArgumentNullException(nameof(extensions));

      BaseName = baseName;
      Extensions = new List<string>(extensions);
      this.extensionCharacter = extensionCharacter;
    }

    /// <summary>
    /// Parses a given filename and creates an instance of <see cref="FilenameExtensionBuilder"/>.
    /// </summary>
    /// <returns>A filename extension builder.</returns>
    /// <param name="filename">The filename to parse.</param>
    /// <param name="extensionCharacter">The extension separator character.</param>
    public static FilenameExtensionBuilder Parse(string filename,
                                                 char extensionCharacter = DefaultExtensionSeparator)
    {
      if(filename == null)
        throw new ArgumentNullException(nameof(filename));

      var parts = filename.Split(extensionCharacter);
      string baseName;
      IReadOnlyList<string> extensions;

      if(parts.Length == 0
          || (parts.Length == 1 && parts[0].Length == 0)
          || (parts.Length > 1 && parts[0].Length == 0 && parts[1].Length == 0))
      {
        throw new ArgumentException(Resources.ExceptionMessages.FilenameMustNotBeEmpty);
      }

      if(parts[0].Length == 0 && parts[1].Length > 0)
      {
        baseName = String.Concat(extensionCharacter, parts[1]);
        extensions = parts.Skip(2).ToArray();
      }
      else
      {
        baseName = parts[0];
        extensions = parts.Skip(1).ToArray();
      }

      return new FilenameExtensionBuilder(baseName, extensions, extensionCharacter);
    }
  }
}