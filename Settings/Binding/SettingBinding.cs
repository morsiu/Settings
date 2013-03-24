namespace TheSettings.Binding
{
    public class SettingBinding : ISettingBinding
    {
        private readonly IValueAdapter _targetAdapter;
        private readonly IValueAdapter _settingAdapter;

        public SettingBinding(IValueAdapter targetAdapter, IValueAdapter settingAdapter)
        {
            _targetAdapter = targetAdapter;
            _settingAdapter = settingAdapter;
            _targetAdapter.ValueChangedCallback = _settingAdapter.SetValue;
            _settingAdapter.ValueChangedCallback = _targetAdapter.SetValue;
        }

        public void UpdateTarget()
        {
            var value = _settingAdapter.GetValue();
            if (value == SettingsConstants.NoValue)
            {
                return;
            }
            _targetAdapter.SetValue(value);
        }
    }
}
