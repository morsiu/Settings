﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Windows;
using TheSettings;
using TheSettings.Binding.Accessors;
using TheSettings.Stores;
using TheSettings.Wpf;
using TheSettings.Wpf.Binding;
using TheSettings.Wpf.Binding.Adapters;

namespace Sample
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SelectedItemsBinding.AdapterFactories.Add(MultiSelectorSelectedItemsAdapter.Create);
            var settingsStore = new SettingsStore();
            Settings.CurrentStoreAccessor = new SingleSettingsStoreAccessor(settingsStore);

            var @namespace = new SettingsNamespace(new SettingsNamespace("Test"), "SelectedItems");
            var value = new[] { 1, 5, 7 };
            settingsStore.SetSetting(@namespace, "SelectedItems", value);
        }
    }
}