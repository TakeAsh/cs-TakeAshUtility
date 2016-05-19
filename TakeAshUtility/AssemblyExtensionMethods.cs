using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TakeAshUtility {

    public static class AssemblyExtensionMethods {

        public static T GetAttribute<T>(this Assembly assembly)
            where T : Attribute {

            if (assembly == null) {
                return null;
            }
            return Attribute.GetCustomAttribute(assembly, typeof(T)) as T;
        }
    }
}
