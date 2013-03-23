using System;
using System.ComponentModel;
using System.Reflection;

namespace Settings.Binding.ValueAdapters
{
    public sealed class ClrPropertyAdapter : IValueAdapter, IDisposable
    {
        private readonly object _target;
        private readonly PropertyInfo _propertyInfo;
        private bool _isDisposed;
        private Action<object> _valueChangedCallback;

        public ClrPropertyAdapter(object target, PropertyInfo propertyInfo)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
            _propertyInfo = propertyInfo;
            _target = target;
            var notifyingTarget = _target as INotifyPropertyChanged;
            if (notifyingTarget != null)
            {
                _valueChangedCallback = newValue => { };
                PropertyChangedEventManager.AddHandler(notifyingTarget, PropertyChangedHandler, _propertyInfo.Name);
            }
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
            return _propertyInfo.CanRead
                ? _propertyInfo.GetValue(_target)
                : SettingsConstants.NoValue;
        }

        public void SetValue(object value)
        {
            FailIfDisposed();
            if (_propertyInfo.CanWrite)
            {
                _propertyInfo.SetValue(_target, value);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            var notifyingTarget = _target as INotifyPropertyChanged;
            if (notifyingTarget != null)
            {
                PropertyChangedEventManager.RemoveHandler(notifyingTarget, PropertyChangedHandler, _propertyInfo.Name);
            }
            _isDisposed = true;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var value = GetValue();
            _valueChangedCallback(value);
        }

        private void FailIfDisposed()
        {
            if (_isDisposed) throw new ObjectDisposedException("ClrPropertyAdapter");
        }
    }
}
