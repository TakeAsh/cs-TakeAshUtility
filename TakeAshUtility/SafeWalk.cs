using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    /// <summary>
    /// Skip unauthorized directory
    /// </summary>
    /// <remarks>
    /// [.net - Directory.EnumerateFiles => UnauthorizedAccessException - Stack Overflow](http://stackoverflow.com/questions/5098011)
    /// </remarks>
    public static class SafeWalk {

        public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOpt) {
            try {
                var dirFiles = Enumerable.Empty<string>();
                if (searchOpt == SearchOption.AllDirectories) {
                    dirFiles = Directory.EnumerateDirectories(path)
                        .SelectMany(x => EnumerateFiles(x, searchPattern, searchOpt));
                }
                return dirFiles.Concat(Directory.EnumerateFiles(path, searchPattern));
            }
#pragma warning disable 0168
            catch (UnauthorizedAccessException ex) {
                return Enumerable.Empty<string>();
            }
#pragma warning restore 0168
        }
    }
}
