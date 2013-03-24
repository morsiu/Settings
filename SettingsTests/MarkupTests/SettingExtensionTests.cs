using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettings;
using TheSettings.Binding.Wpf;
using TheSettings.Binding.Wpf.Markup;

namespace TheSettingsTests.MarkupTests
{
    [TestClass]
    public class SettingExtensionTests
    {
        [TestInitialize]
        public void Initialize()
        {
            Settings.CurrentStoreAccessor = new SingleSettingsStoreAccessor(new SettingsStore());
        }

        [TestMethod]
        public void ShouldCrateBindingThatPassesChangedValueToSettingsStore()
        {
            var tree = new SettingBinding();
            tree.DataContext = 100;
            var value = Settings.CurrentStoreAccessor.GetSetting(null, new SettingsNamespace("Namespace"), "Setting");
            Assert.AreEqual(100, value);
        }

        [TestMethod]
        public void ShouldSetStoredValueAfterBindingCreation()
        {
            Settings.CurrentStoreAccessor.SetSetting(null, new SettingsNamespace("Namespace"), "Setting", 100);
            var tree = new SettingBinding();
            Assert.AreEqual(100, tree.DataContext);
        }
    }
}
