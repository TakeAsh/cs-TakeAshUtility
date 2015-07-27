using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TakeAshUtility {

    public class PropertyChangedWithValueEventArgs<TValue> :
        EventArgs {

        public PropertyChangedWithValueEventArgs(
            string propertyName,
            TValue newValue = default(TValue),
            TValue oldValue = default(TValue)
        )
            : base() {
            this.PropertyName = propertyName;
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }

        public string PropertyName { get; private set; }
        public TValue NewValue { get; private set; }
        public TValue OldValue { get; private set; }
    }

    public delegate void PropertyChangedWithValueEventHandler<TValue>(
        object sender,
        PropertyChangedWithValueEventArgs<TValue> e
    );

    public interface IPropertyChangedWithValue<TValue> {
        event PropertyChangedWithValueEventHandler<TValue> PropertyChangedWithValue;
    }

    public static class IPropertyChangedWithValueExtensionMethods {

        const string EventHandlerName = "PropertyChangedWithValue";

        public static void NotifyPropertyChanged<TValue>(
            this IPropertyChangedWithValue<TValue> sender,
            string propertyName,
            TValue newValue = default(TValue),
            TValue oldValue = default(TValue)
        ) {
            if (sender == null || String.IsNullOrEmpty(propertyName)) {
                return;
            }
            var handler = sender.GetDelegate(EventHandlerName)
                .GetHandler<PropertyChangedWithValueEventHandler<TValue>>();
            if (handler == null) {
                return;
            }
            handler(
                sender,
                new PropertyChangedWithValueEventArgs<TValue>(propertyName, newValue, oldValue)
            );
        }

        public static void SetPropertyAndNotifyIfChanged<TValue>(
            this IPropertyChangedWithValue<TValue> sender,
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
