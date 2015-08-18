using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Media.Imaging;

namespace TakeAshUtility {

    public static class ResourceHelper {

        public static BitmapImage GetImage(string filename, string assembly = null) {
            if (filename == null) {
                return null;
            }
            assembly = assembly ?? Assembly.GetCallingAssembly().GetName().Name;
            return new BitmapImage(new Uri("/" + assembly + ";component/" + filename, UriKind.Relative));
        }

        public static BitmapImage GetEmbeddedImage(string filename) {
            if (String.IsNullOrEmpty(filename)) {
                return null;
            }
            using (var stream = GetResourceStream(Assembly.GetCallingAssembly(), filename)) {
                if (stream == null) {
                    return null;
                }
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }

        /// <summary>
        /// Get text of embedded resource file
        /// </summary>
        /// <param name="filename">resource file name</param>
        /// <param name="encoding">encoding of file content. Default is UTF8.</param>
        /// <returns>content of resource file</returns>
        /// <remarks>The resource file must be embedded.</remarks>
        public static string GetText(string filename, Encoding encoding = null) {
            if (filename == null) {
                return null;
            }
            encoding = encoding ?? Encoding.UTF8;
            using (var stream = GetResourceStream(Assembly.GetCallingAssembly(), filename))
            using (var reader = new StreamReader(stream, encoding)) {
                return reader.ReadToEnd();
            }
        }

        private static Stream GetResourceStream(Assembly assembly, string filename) {
            var directory = Path.GetDirectoryName(filename).Replace('-', '_');
            var file = Path.GetFileName(filename);
            var resourceName = Path.Combine(assembly.GetName().Name, directory, file).Replace('\\', '.').Replace('/', '.');
            return assembly.GetManifestResourceStream(resourceName);
        }

        public static ResourceManager GetResourceManager(Assembly assembly) {
            if (assembly == null) {
                return null;
            }
            var resourceName = assembly.GetName().Name + ".Properties.Resources";
            if (assembly.GetManifestResourceInfo(resourceName + ".resources") == null) {
                return null;
            }
            return new ResourceManager(resourceName, assembly);
        }

        public static ResourceManager GetResourceManager(Type type) {
            if (type == null) {
                return null;
            }
            return GetResourceManager(type.Assembly);
        }

        public static ResourceManager GetResourceManager(string assemblyName) {
            if (String.IsNullOrEmpty(assemblyName)) {
                return null;
            }
            return GetResourceManager(AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(asm => asm.GetName().Name == assemblyName)
                .FirstOrDefault()
            );
        }
    }
}
