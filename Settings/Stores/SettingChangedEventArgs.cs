using System;

namespace TheSettings.Stores
{
    public class SettingChangedEventArgs : EventArgs
    {
        public SettingChangedEventArgs(SettingsNamespace @namespace, string settingName, object value)
        {
            Namespace = @namespace;
            SettingName = settingName;
            Value = value;
        }

        public SettingsNamespace Namespace { get; private set; }

        public string SettingName { get; private set; }

        public object Value { get; private set; }
    }
}