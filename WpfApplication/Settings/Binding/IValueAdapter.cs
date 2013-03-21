using System;

namespace WpfApplication.Settings.Binding
{
    public interface IValueAdapter
    {
        Action<object> ValueChangedCallback { set; }

        object GetValue();

        void SetValue(object value);
    }
}
