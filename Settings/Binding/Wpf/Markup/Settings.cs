using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace TheSettings.Binding.Wpf.Markup
{
    [ContentProperty("Settings")]
    public static class Settings
    {
        private const FrameworkPropertyMetadataOptions InheritanceFlags =
            FrameworkPropertyMetadataOptions.Inherits;

        public static readonly DependencyProperty NamespaceProperty =
            DependencyProperty.RegisterAttached(
                "Namespace", 
                typeof(SettingsNamespace), 
                typeof(Settings), 
                new FrameworkPropertyMetadata(SettingsNamespace.None, InheritanceFlags));

        public static readonly DependencyProperty SettingsProperty =
            DependencyProperty.RegisterAttached(
                "SettingsInternal", // Name is not Settings to force XAML reader to use GetSettings method to obtain value.
                typeof (SettingBindingCollection),
                typeof (Settings),
                new FrameworkPropertyMetadata(SettingBindingCollection.UnsetValue));

        internal static readonly DependencyProperty InitializerProperty =
            DependencyProperty.RegisterAttached(
                "Initializer",
                typeof(SettingsInitializer),
                typeof(Settings),
                new FrameworkPropertyMetadata(null, InheritanceFlags));

        static Settings()
        {
            CurrentStoreAccessor = new SingleSettingsStoreAccessor(new NullSettingsStore());
        }

        internal static SettingsInitializer GetInitializer(DependencyObject element)
        {
            return (SettingsInitializer) element.GetValue(InitializerProperty);
        }

        internal static void SetInitializer(DependencyObject element, SettingsInitializer value)
        {
            element.SetValue(InitializerProperty, value);
        }

        public static SettingBindingCollection GetSettings(DependencyObject element)
        {
            var value = (SettingBindingCollection)element.GetValue(SettingsProperty);
            if (value == SettingBindingCollection.UnsetValue)
            {
                value = new SettingBindingCollection(element);
                SetSettings(element, value);
            }
            return value;
        }

        public static void SetSettings(DependencyObject element, SettingBindingCollection value)
        {
            element.SetValue(SettingsProperty, value);
        }

        public static SettingsNamespace GetNamespace(DependencyObject element)
        {
            return (SettingsNamespace)element.GetValue(NamespaceProperty);
        }

        public static void SetNamespace(DependencyObject element, SettingsNamespace value)
        {
            element.SetValue(NamespaceProperty, value);
        }

        public static ISettingsStoreAccessor CurrentStoreAccessor { get; set; }

        internal static void DebugValue(string name, DependencyObject element, ValueSource source, object value)
        {
            Debug.Print("{2}, {3}, \"{1}\", {0}", element.GetType(), value, source.BaseValueSource, name);
        }
    }
}
