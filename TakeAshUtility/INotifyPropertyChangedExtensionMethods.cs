using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TakeAshUtility {

    /// <summary>
    /// implement PropertyChangedEventHandler caller
    /// </summary>
    /// <remarks>
    /// [How to get a delegate object from an EventInfo? - Stack Overflow](http://stackoverflow.com/questions/3783267)
    /// </remarks>
    public static class INotifyPropertyChangedExtensionMethods {

        const string PropertyChangedHandlerName = "PropertyChanged";

        public static void NotifyPropertyChanged(this INotifyPropertyChanged sender, string propertyName = "") { // [CallerMemberName]
            if (String.IsNullOrEmpty(propertyName)) {
                return;
            }
            sender.NotifyPropertyChanged(new[] { propertyName });
        }

        public static void NotifyPropertyChanged(this INotifyPropertyChanged sender, IEnumerable<string> propertyNames) {
            if (propertyNames == null) {
                return;
            }
            var handler = sender.GetDelegate(PropertyChangedHandlerName)
                .GetHandler<PropertyChangedEventHandler>();
            if (handler == null) {
                return;
            }
            propertyNames.Where(name => !String.IsNullOrEmpty(name))
                .ToList()
                .ForEach(name => handler(sender, new PropertyChangedEventArgs(name)));
        }
    }
}
