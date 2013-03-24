namespace TheSettings
{
    public interface ISettingsStore
    {
        object GetSetting(SettingsNamespace @namespace, string name);

        void SetSetting(SettingsNamespace @namespace, string name, object value);
    }
}
