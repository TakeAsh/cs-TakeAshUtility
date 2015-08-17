using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {

    public static class ApplicationSettingsBaseExtensionMethods {

        public static string ToDetail(this ApplicationSettingsBase settings) {
            if (settings == null) {
                return null;
            }
            return String.Join(
                ", ",
                settings.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(pi => pi.GetIndexParameters().Length == 0)
                    .Select(pi => pi.Name + ":{" + pi.GetValue(settings, null) + "}")
            );
        }
    }
}
