using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TheSettings;
using TheSettings.Binding.Wpf;
using TheSettings.Binding.Wpf.Markup;
using TheSettingsTests.Infrastructure;
using TheSettingsTests.MarkupTests.SettingExtensionTests.Windows;

namespace TheSettingsTests.MarkupTests.SettingExtensionsTests
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
