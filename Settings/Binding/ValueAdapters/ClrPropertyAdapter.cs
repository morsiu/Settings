using System;
using System.ComponentModel;
using System.Reflection;

namespace Settings.Binding.ValueAdapters
{
    public class ClrPropertyAdapter : IValueAdapter
    {
        private readonly Action<object> _setter;
        private readonly Func<object> _getter;
        private Action<object> _valueChangedCallback;

        public ClrPropertyAdapter(object target, PropertyInfo propertyInfo)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (propertyInfo == null) throw new ArgumentNullException("propertyInfo");
            if (propertyInfo.CanWrite)
            {
                _setter = value => propertyInfo.SetValue(target, value);
            }
            else
            {
                _setter = value => { };
            }
            if (propertyInfo.CanRead)
            {
                _getter = () => propertyInfo.GetValue(target);
            }
            else
            {
                _getter = () => SettingsConstants.NoValue; 
            }
            var notifyingTarget = target as INotifyPropertyChanged;
            if (notifyingTarget != null)
            {
                PropertyChangedEventManager.AddHandler(notifyingTarget, PropertyChangedHandler, propertyInfo.Name);
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var value = GetValue();
            _valueChangedCallback(value);
        }

        public Action<object> ValueChangedCallback
        {
            set { _valueChangedCallback = value; }
        }

        public object GetValue()
        {
            return _getter();
        }

        public void SetValue(object value)
        {
            _setter(value);
        }
    }
}
