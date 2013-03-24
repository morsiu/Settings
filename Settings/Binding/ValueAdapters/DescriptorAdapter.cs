using System;
using System.ComponentModel;

namespace TheSettings.Binding.ValueAdapters
{
    public class DescriptorAdapter : IValueAdapter, IDisposable
    {
        private readonly PropertyDescriptor _descriptor;
        private readonly object _target;
        private bool _isDisposed;
        private Action<object> _valueChangedCallback;

        public DescriptorAdapter(object target, PropertyDescriptor descriptor)
        {
            _target = target;
            _descriptor = descriptor;
            _valueChangedCallback = newValue => { };
            descriptor.AddValueChanged(target, OnValueChanged);
        }

        private void OnValueChanged(object sender, EventArgs e)
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
            var value = _descriptor.GetValue(_target);
            return value;
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            _descriptor.SetValue(_target, value);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _descriptor.RemoveValueChanged(_target, OnValueChanged);
            _isDisposed = true;
        }

        private void FailIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("DescriptorAdapter");
        }
    }
}
