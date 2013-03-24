Main interfaces (starting from Settings namespace):
ISettingsStore - for storage of settings
    see SettingsStore for sample in-memory implementation.
Binding\ISettingBinding - for setting bindings
    see Binding\SettingBinding for sample implementation.
Binding\ISettingBindingsProvider - for generating setting bindings
    see Binding\Wpf\Markup\Binding and DataGridColumnsBinding
Binding\IValueAdapter - for endpoints used by setting bindings, 
    see Binding\ValueAdapters for sample implementations.

Namespace Settings\Binding\Wpf contains all stuff that allows settings to be uses with WPF.

Namespace Settings\Binding\Wpf\Markup contains all classes that should be visible to WPF
- that is markup extensions, binding providers and attached properties through Settings static class.

By default store reachable through Settings.CurrentStoreAccessor is a NullSettingsStore wrapped in SingleSettingsStoreAccessor.