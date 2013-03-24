using System;
using System.ComponentModel;
using System.Windows;

namespace TheSettings.Binding.ValueAdapters
{
    public sealed class DependencyPropertyAdapter : IValueAdapter, IDisposable
    {
        private readonly DependencyProperty _property;
        private readonly DependencyPropertyDescriptor _descriptor;
        private readonly DependencyObject _target;
        private bool _isDisposed;
        private Action<object> _valueChangedCallback;

        public DependencyPropertyAdapter(DependencyObject target, DependencyProperty property)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (property == null) throw new ArgumentNullException("property");
            _target = target;
            _property = property;
            _descriptor = DependencyPropertyDescriptor.FromProperty(property, property.OwnerType);
            _valueChangedCallback = newValue => { };
            // TODO: Use weak event pattern here
            _descriptor.AddValueChanged(target, PropertyChangedHandler);
        }

        private void PropertyChangedHandler(object sender, EventArgs e)
        {
            var value = GetValue();
            _valueChangedCallback(value);
        }

        public Action<object> ValueChangedCallback
        {
            set 
            {
                FailIfDisposed();
                _valueChangedCallback = value ?? (newValue => { });
            }
        }

        public object GetValue()
        {
            FailIfDisposed();
            return _target.GetValue(_property);
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            if (!_property.ReadOnly)
            {
                _target.SetValue(_property, value);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _descriptor.RemoveValueChanged(_target, PropertyChangedHandler);
            _isDisposed = true;
        }

        private void FailIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("DependencyPropertyAdapter");
        }
    }
}
