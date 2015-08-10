using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TakeAshUtility {
    
    public static class EventHandlerExtensionMethods {

        public static Delegate GetDelegate(this Object obj, string eventHandlerName) {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;
            FieldInfo field;
            return obj == null ||
                String.IsNullOrEmpty(eventHandlerName) ||
                (field = obj.GetType().GetField(eventHandlerName, flags)) == null ?
                    null :
                    field.GetValue(obj) as Delegate;
        }

        public static T GetHandler<T>(this Delegate delgate)
            where T : class {

            return delgate == null ?
                null :
                delgate.GetInvocationList()
                    .Select(del => del as T)
                    .Where(handler => handler != null)
                    .FirstOrDefault();
        }
    }
}
