using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TakeAshUtility {

    /// <summary>
    /// implement PropertyChangedEventHandler caller
    /// </summary>
    /// <remarks>
    /// Usage:
    /// <code>
    /// public class SampleClass :
    ///     INotifyPropertyChanged {
    /// 
    ///     private int _id;
    ///     private string _name;
    /// 
    ///     public bool IsDirty { get; private set; }
    /// 
    ///     public int ID {
    ///         get { return _id; }
    ///         set {
    ///             _id = value;
    ///             this.NotifyPropertyChanged("ID"); // Notify whenever
    ///         }
    ///     }
    /// 
    ///     public string Name {
    ///         get { return _name; }
    ///         set { IsDirty |= this.SetField(ref _name, value, "Name"); } // Notify if changed
    ///     }
    /// 
    /// #pragma warning disable 0067
    ///     public event PropertyChangedEventHandler PropertyChanged;
    /// #pragma warning restore 0067
    /// 
    /// }
    /// </code>
    /// <list type="bullet">
    /// <item>[How to get a delegate object from an EventInfo? - Stack Overflow](http://stackoverflow.com/questions/3783267)</item>
    /// <item>[c# - Implementing INotifyPropertyChanged - does a better way exist? - Stack Overflow](http://stackoverflow.com/questions/1315621/)</item>
    /// </list>
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
            var handlers = sender.GetDelegate(PropertyChangedHandlerName)
                .GetHandlers<PropertyChangedEventHandler>();
            if (handlers == null) {
                return;
            }
            propertyNames.Where(name => !String.IsNullOrEmpty(name))
                .ForEach(name => handlers.ForEach(hander => hander(sender, new PropertyChangedEventArgs(name))));
        }

        public static bool SetField<T>(
            this INotifyPropertyChanged sender,
            ref T field,
            T value,
            string propertyName = ""
        ) {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }
            field = value;
            sender.NotifyPropertyChanged(propertyName);
            return true;
        }

        public static bool SetField<T>(
            this INotifyPropertyChanged sender,
            ref T field,
            T value,
            IEnumerable<string> propertyNames
        ) {
            if (EqualityComparer<T>.Default.Equals(field, value)) {
                return false;
            }
            field = value;
            sender.NotifyPropertyChanged(propertyNames);
            return true;
        }
    }
}
