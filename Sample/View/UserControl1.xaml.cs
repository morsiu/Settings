// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TheSettings;
using TheSettings.Binding.Accessors;
using TheSettings.Stores;
using TheSettings.Wpf;

namespace Sample.View
{
    public partial class UserControl1 : UserControl
    {
        private readonly SettingsStore _store;

        public UserControl1()
        {
            InitializeComponent();
            var accessor = (SingleSettingsStoreAccessor)Settings.CurrentStoreAccessor;
            _store = (SettingsStore)accessor.Store;
            _store.SettingChanged += (o, e) => RefreshSettings();
        }

        private void RefreshSettingsSource(object sender, RoutedEventArgs e)
        {
            RefreshSettings();
        }

        private void RefreshSettings()
        {
            var allSettings = _store.SettingsByNamespace
                .SelectMany(
                    n => n.Value.Settings,
                    (n, c) => new { Namespace = n.Key, Key = c.Key, Value = c.Value })
                .ToList();
            SettingsDataGrid.ItemsSource = allSettings;
        }
    }
}
