using System;
using TheSettings.Binding.Wpf;

namespace TheSettings.Binding.ValueAdapters
{
    public class SettingAdapter : IValueAdapter
    {
        private readonly ISettingsStoreAccessor _storeAccessor;
        private readonly SettingsNamespace _settingNamespace;
        private readonly string _settingName;
        private readonly object _storeKey;

        public SettingAdapter(
            ISettingsStoreAccessor storeAccessor,
            object storeKey,
            SettingsNamespace @namespace,
            string name)
        {
            _storeAccessor = storeAccessor;
            _settingNamespace = @namespace;
            _settingName = name;
            _storeKey = storeKey;
        }

        public Action<object> ValueChangedCallback
        {
            set { }
        }

        public object GetValue()
        {
            return _storeAccessor.GetSetting(_storeKey, _settingNamespace, _settingName);
        }

        public void SetValue(object value)
        {
            _storeAccessor.SetSetting(_storeKey, _settingNamespace, _settingName, value);
        }
    }
}
