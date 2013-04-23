// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheSettings;
using TheSettings.Stores;

namespace TheSettingsTests.StoreTests
{
    [TestClass]
    public class PersistentStoreTests : DelegatingStoreTestSuite
    {
        [TestMethod]
        public void ShouldNotStoreSettingsIfPolicyProhibits()
        {
            var innerStore = new Mock<ISettingsStore>();
            bool wasCalled = false;
            var store = new PersistentStore(innerStore.Object, settings => false, settings => { wasCalled = true; });
            store.SetSetting(null, null, null);
            Assert.IsFalse(wasCalled);
        }

        [TestMethod]
        public void ShouldPassSettingsToPolicy()
        {
            var innerStore = new Mock<ISettingsStore>();
            var setting = new Setting(new SettingsNamespace("NS"), "Name", new object());
            Setting actualSetting = null;
            var store = new PersistentStore(
                innerStore.Object,
                settings =>
                {
                    actualSetting = settings.FirstOrDefault();
                    return true;
                },
                settings => { });
            store.SetSetting(setting.Namespace, setting.Name, setting.Value);
            Assert.AreEqual(setting, actualSetting);
        }

        [TestMethod]
        public void ShouldStoreSettingsIfPolicySaysSo()
        {
            var innerStore = new Mock<ISettingsStore>();
            bool wasCalled = false;
            var store = new PersistentStore(innerStore.Object, settings => true, settings => { wasCalled = true; });
            store.SetSetting(null, null, null);
            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void ShouldPassSettingsForStore()
        {
            var innerStore = new Mock<ISettingsStore>();
            var setting = new Setting(new SettingsNamespace("NS"), "Name", new object());
            Setting actualSetting = null;
            var store = new PersistentStore(innerStore.Object, settings => true, settings => { actualSetting = settings.FirstOrDefault(); });
            store.SetSetting(setting.Namespace, setting.Name, setting.Value);
            Assert.AreEqual(setting, actualSetting);
        }

        [TestMethod]
        public void ShouldClearSettingsAfterStore()
        {
            var innerStore = new Mock<ISettingsStore>();
            var setting = new Setting(new SettingsNamespace("NS"), "Name", new object());
            var settingsList = new List<List<Setting>>();
            var store = new PersistentStore(innerStore.Object, settings => true, settings => settingsList.Add(settings.ToList()));
            store.SetSetting(setting.Namespace, setting.Name, setting.Value);
            store.SetSetting(null, null, null);
            CollectionAssert.AreNotEquivalent(settingsList[0], settingsList[1]);
        }

        protected override ISettingsStore ProvideStore(ISettingsStore innerStore)
        {
            return new PersistentStore(innerStore, settings => true, settings => { });
        }
    }
}