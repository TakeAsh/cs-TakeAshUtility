using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (String.IsNullOrEmpty(path)) {
                return Enumerable.Empty<string>();
            }
            try {
                var dirFiles = Enumerable.Empty<string>();
                if (searchOpt == SearchOption.AllDirectories) {
                    dirFiles = Directory.EnumerateDirectories(path)
                        .SelectMany(x => EnumerateFiles(x, searchPattern, searchOpt));
                }
                return dirFiles.Concat(Directory.EnumerateFiles(path, searchPattern));
            }
            catch (Exception ex) {
                Debug.Print(ex.GetAllMessages());
                return Enumerable.Empty<string>();
            }
        }
    }
}
