// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using TheSettings.Binding;
using TheSettings.Binding.Accessors;
using TheSettings.Wpf.Infrastructure;
using TheSettings.Wpf.Initialization;

namespace TheSettings.Wpf
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
