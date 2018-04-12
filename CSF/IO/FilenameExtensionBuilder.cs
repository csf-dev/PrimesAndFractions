using System;
using System.Collections.Generic;
using System.Linq;

namespace CSF.IO
{
    public class FilenameExtensionBuilder
    {
        readonly char extensionCharacter;
        string baseName;

        public string BaseName
        {
            get {  return baseName; }
            set {
                if(value == null)
                    throw new ArgumentNullException(nameof(value));
                if(value.Length == 0)
                    throw new ArgumentException(Resources.ExceptionMessages.FileBaseNameMustNotBeEmpty);

                baseName = value;
            }
        }

        public IList<string> Extensions { get; }

        public override string ToString()
        {
            var parts = new List<string>(Extensions);
            parts.Insert(0, BaseName);

            return String.Join(extensionCharacter.ToString(), parts.Where(x => x != null));
        }

        public FilenameExtensionBuilder(string baseName, IReadOnlyList<string> extensions, char extensionCharacter = '.')
        {
            if(extensions == null)
                throw new ArgumentNullException(nameof(extensions));

            BaseName = baseName;
            Extensions = new List<string>(extensions);
            this.extensionCharacter = extensionCharacter;
        }

        public static FilenameExtensionBuilder Parse(string filename, char extensionCharacter = '.')
        {
            if(filename == null)
                throw new ArgumentNullException(nameof(filename));

            var parts = filename.Split(extensionCharacter);
            string baseName;
            IReadOnlyList<string> extensions;

            if (parts.Length == 0
                || (parts.Length == 1 && parts[0].Length == 0)
                || (parts.Length > 1 && parts[0].Length == 0 && parts[1].Length == 0))
            {
                throw new ArgumentException(Resources.ExceptionMessages.FilenameMustNotBeEmpty);
            }

            if (parts[0].Length == 0 && parts[1].Length > 0)
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