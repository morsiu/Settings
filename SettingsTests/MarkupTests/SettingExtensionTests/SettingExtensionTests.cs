// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using TheSettings.Binding.Accessors;
using TheSettings.Stores;
using TheSettings.Wpf;
using TheSettingsTests.Infrastructure;
using TheSettingsTests.MarkupTests.SettingExtensionTests.Windows;

namespace TheSettingsTests.MarkupTests.SettingExtensionTests
{
    [TestClass]
    public class SettingExtensionTests : GuiTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Settings.CurrentStoreAccessor = new SingleSettingsStoreAccessor(new SettingsStore());
        }

        [TestMethod]
        public void ShouldCreateBindingThatPassesChangedValueToSettingsStore()
        {
            var window = CreateShowThenCloseAtCleanup<SettingBinding>();
            window.DataContext = 100;
            var value = Settings.CurrentStoreAccessor.GetSetting(null, new SettingsNamespace("Namespace"), "Setting");
            Assert.AreEqual(100, value);
        }

        [TestMethod]
        public void ShouldSetStoredValueAfterBindingCreation()
        {
            Settings.CurrentStoreAccessor.SetSetting(null, new SettingsNamespace("Namespace"), "Setting", 100);
            var window = CreateShowThenCloseAtCleanup<SettingBinding>();
            Assert.AreEqual(100, window.DataContext);
        }

        [TestMethod]
        public void ShouldCreateThroughStyleBindingThatPassesChangedValueToSettingsStore()
        {
            var window = CreateShowThenCloseAtCleanup<SettingBinding>();
            window.Child1DataContext = 200;
            var value = Settings.CurrentStoreAccessor.GetSetting(null, new SettingsNamespace("Namespace"), "StyleSetting");
            Assert.AreEqual(200, value);
        }

        [TestMethod]
        public void ShouldSetStoredValueAfterBindingCreatedThroughStyle()
        {
            Settings.CurrentStoreAccessor.SetSetting(null, new SettingsNamespace("Namespace"), "StyleSetting", 200);
            var window = CreateShowThenCloseAtCleanup<SettingBinding>();
            Assert.AreEqual(200, window.Child1DataContext);
        }
    }
}
