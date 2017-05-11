using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace TakeAshUtility {

    /// <summary>
    /// Get Extension or MIME Type from registry.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>[C# Get file extension by content type - Stack Overflow](http://stackoverflow.com/questions/23087808/)</item>
    /// <item>[Snippet: Mime types and file extensions - Articles and information on C# and .NET development topics - Cyotek](https://www.cyotek.com/blog/mime-types-and-file-extensions)</item>
    /// </list>
    /// </remarks>
    public static class MIME {

        public const string DefaultExtension = ".bin";
        public const string DefaultMIMEType = "application/octet-stream";

        /// <summary>
        /// Get the default extension from the mime type.
        /// </summary>
        /// <param name="mimeType">the mime type.</param>
        /// <returns>the default extension.</returns>
        public static string GetDefaultExtension(string mimeType) {
            if (String.IsNullOrEmpty(mimeType)) {
                return DefaultExtension;
            }
            var key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            var value = key == null ?
                null :
                key.GetValue("Extension", null);
            return value == null ?
                DefaultExtension :
                value.ToString();
        }

        /// <summary>
        /// Get the MIME Type from the file extension.
        /// </summary>
        /// <param name="extension">the file extension.</param>
        /// <returns>the MIME Type.</returns>
        public static string GetMimeTypeFromExtension(string extension) {
            if (String.IsNullOrEmpty(extension)) {
                return DefaultMIMEType;
            }
            if (!extension.StartsWith(".")) {
                extension = "." + extension;
            }
            var key = Registry.ClassesRoot.OpenSubKey(extension, false);
            var value = key == null ?
                null :
                key.GetValue("Content Type", null);
            return value == null ?
                DefaultMIMEType :
                value.ToString();
        }
    }
}
