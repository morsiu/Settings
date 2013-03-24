using System;

namespace TheSettings.Binding.ValueAdapters
{
    public class SettingAdapter : IValueAdapter
    {
        private readonly ISettingsStore _store;

        private readonly SettingsNamespace _settingNamespace;

        private readonly string _settingName;

        public SettingAdapter(ISettingsStore store, SettingsNamespace @namespace, string name)
        {
            _store = store;
            _settingNamespace = @namespace;
            _settingName = name;
        }

        public Action<object> ValueChangedCallback
        {
            set { }
        }

        public object GetValue()
        {
            return _store.GetSetting(_settingNamespace, _settingName);
        }

        public void SetValue(object value)
        {
            _store.SetSetting(_settingNamespace, _settingName, value);
        }
    }
}
