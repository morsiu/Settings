using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Settings.Binding.ValueAdapters
{
    public class DescriptorAdaptor : IValueAdapter
    {
        private readonly PropertyDescriptor _descriptor;
        private readonly object _target;
        private Action<object> _valueChangedCallback;

        public DescriptorAdaptor(object target, PropertyDescriptor descriptor)
        {
            _target = target;
            _descriptor = descriptor;
            descriptor.AddValueChanged(target, OnValueChanged);
        }

        private void OnValueChanged(object sender, EventArgs e)
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
            var value = _descriptor.GetValue(_target);
            return value;
        }

        public void SetValue(object value)
        {
            _descriptor.SetValue(_target, value);
        }
    }
}
