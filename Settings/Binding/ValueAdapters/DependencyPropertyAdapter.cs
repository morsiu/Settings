using System;
using System.ComponentModel;
using System.Windows;

namespace Settings.Binding.ValueAdapters
{
    public class DependencyPropertyAdapter : IValueAdapter
    {
        private readonly DependencyProperty _property;
        private readonly DependencyPropertyDescriptor _descriptor;
        private readonly DependencyObject _target;
        private Action<object> _valueChangedCallback;

        public DependencyPropertyAdapter(DependencyObject target, DependencyProperty property)
        {
            _target = target;
            _property = property;
            _descriptor = DependencyPropertyDescriptor.FromProperty(property, property.OwnerType);
            // TODO: Use weak event pattern here
            _descriptor.AddValueChanged(target, PropertyChangedHandler);
        }

        private void PropertyChangedHandler(object sender, EventArgs e)
        {
            if (_valueChangedCallback != null)
            {
                var value = GetValue();
                _valueChangedCallback(value);
            }
        }

        public Action<object> ValueChangedCallback
        {
            set { _valueChangedCallback = value; }
        }

        public object GetValue()
        {
            return _target.GetValue(_property);
        }

        public void SetValue(object value)
        {
            _target.SetValue(_property, value);
        }
    }
}
