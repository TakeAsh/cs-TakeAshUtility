using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace TakeAshUtility {

    public static class AssemblyHelper {

        private static readonly Regex regPropertyResources = new Regex(@"\.Properties\.");
        private static readonly Regex regLastResources = new Regex(@"\.resources$");

        public static T GetAttribute<T>(this Assembly assembly)
            where T : Attribute {

            if (assembly == null) {
                return null;
            }
            return Attribute.GetCustomAttribute(assembly, typeof(T)) as T;
        }

        /// <summary>
        /// Get Assembly that has specified name from CurrentDomain
        /// </summary>
        /// <param name="name">The name of Assembly</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>not null, if the Assembly is found.</item>
        /// <item>null, if the Assembly is not found.</item>
        /// </list>
        /// </returns>
        public static Assembly GetAssembly(string name) {
            return String.IsNullOrEmpty(name) ?
                null :
                AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Where(assembly => assembly.GetName().Name == name)
                    .FirstOrDefault();
        }

        public static ResourceManager GetResourceManager(this Assembly assembly) {
            var resourceName = assembly.GetManifestResourceNames()
                .Where(name => regPropertyResources.IsMatch(name))
                .FirstOrDefault();
            return String.IsNullOrEmpty(resourceName) ?
                null :
                new ResourceManager(regLastResources.Replace(resourceName, String.Empty), assembly);
        }
    }
}
