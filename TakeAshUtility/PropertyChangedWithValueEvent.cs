using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public class PropertyChangedWithValueEventArgs :
        EventArgs {

        public PropertyChangedWithValueEventArgs(
            string propertyName,
            Object newValue = default(Object),
            Object oldValue = default(Object)
        )
            : base() {
            this.PropertyName = propertyName;
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }

        public string PropertyName { get; private set; }
        public Object NewValue { get; private set; }
        public Object OldValue { get; private set; }

        public override string ToString() {
            return PropertyName + ", New:{" + NewValue + "}, Old:{" + OldValue + "}";
        }
    }

    public delegate void PropertyChangedWithValueEventHandler(
        object sender,
        PropertyChangedWithValueEventArgs e
    );

    public interface INotifyPropertyChangedWithValue {
        event PropertyChangedWithValueEventHandler PropertyChangedWithValue;
    }

    public static class INotifyPropertyChangedWithValueExtensionMethods {

        const string EventHandlerName = "PropertyChangedWithValue";

        public static void NotifyPropertyChanged<TValue>(
            this INotifyPropertyChangedWithValue sender,
            string propertyName,
            TValue newValue = default(TValue),
            TValue oldValue = default(TValue)
        ) {
            if (sender == null || String.IsNullOrEmpty(propertyName)) {
                return;
            }
            var handler = sender.GetDelegate(EventHandlerName)
                .GetHandler<PropertyChangedWithValueEventHandler>();
            if (handler == null) {
                return;
            }
            handler(
                sender,
                new PropertyChangedWithValueEventArgs(propertyName, newValue, oldValue)
            );
        }

        public static void SetPropertyAndNotifyIfChanged<TValue>(
            this INotifyPropertyChangedWithValue sender,
            string propertyName,
            ref TValue field,
            TValue newValue
        ) {
            if (sender == null ||
                String.IsNullOrEmpty(propertyName) ||
                EqualityComparer<TValue>.Default.Equals(field, newValue)) {
                return;
            }
            var oldValue = field;
            field = newValue;
            sender.NotifyPropertyChanged(propertyName, newValue, oldValue);
        }
    }
}
