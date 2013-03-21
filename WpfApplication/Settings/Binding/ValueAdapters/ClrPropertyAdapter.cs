using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Settings.Binding.ValueAdapters
{
    public class ClrPropertyAdapter : IValueAdapter
    {
        private readonly object _target;
        private readonly PropertyInfo _property;
        private Action<object> _valueChangedCallback;

        public ClrPropertyAdapter(object target, PropertyInfo propertyInfo)
        {
            _target = target;
            _property = propertyInfo;
            var notifyingTarget = _target as INotifyPropertyChanged;
            if (notifyingTarget != null)
            {
                PropertyChangedEventManager.AddHandler(notifyingTarget, PropertyChangedHandler, _property.Name);
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
            var value = _property.GetValue(_target);
            return value;
        }

        public void SetValue(object value)
        {
            _property.SetValue(_target, value);
        }
    }
}
