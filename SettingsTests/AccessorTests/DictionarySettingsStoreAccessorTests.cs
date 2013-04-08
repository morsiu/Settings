// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheSettings;
using TheSettings.Binding.Accessors;

namespace TheSettingsTests.AccessorTests
{
    [TestClass]
    public class DictionarySettingsStoreAccessorTests
    {
        private DictionarySettingsStoreAccessor _accessor;

        [TestInitialize]
        public void Initialize()
        {
            _accessor = new DictionarySettingsStoreAccessor();
        }

        [TestMethod]
        public void ShouldGetNoValueWhenGettingSettingAndDefaultStoreIsNotSetAndNoStoreIsSetForKey()
        {
            _accessor.DefaultStore = null;
            var value = _accessor.GetSetting("Key", new SettingsNamespace("NS"), "Setting");
            Assert.AreEqual(SettingsConstants.NoValue, value);
        }

        [TestMethod]
        public void ShouldDoNothingWhenSettingASettingAndDefaultStoreIsNotSetAndNoStoreIsSetForKey()
        {
            _accessor.DefaultStore = null;
            try
            {
                _accessor.SetSetting("Key", new SettingsNamespace("NS"), "Setting", null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ShouldGetSettingFromDefaultStoreWhenNoStoreIsSetForKey()
        {
            var defaultStore = new Mock<ISettingsStore>();
            var expectedValue = 5;
            defaultStore.Setup(s => s.GetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>())).Returns(expectedValue);
            _accessor.DefaultStore = defaultStore.Object;
            
            var value = _accessor.GetSetting("Key", new SettingsNamespace("NS"), "Setting");
            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void ShouldSetSettingToDefaultStoreWhenNoStoreIsSetForKey()
        {
            var defaultStore = new Mock<ISettingsStore>();
            _accessor.DefaultStore = defaultStore.Object;

            _accessor.SetSetting("Key", new SettingsNamespace("NS"), "Setting", null);
            defaultStore.Verify(s => s.SetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.IsAny<object>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ShouldGetValueFromStoreOfGivenKey()
        {
            var store = new Mock<ISettingsStore>();
            var expectedValue = 5;
            store.Setup(s => s.GetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>())).Returns(expectedValue);
            _accessor.Set("Key", store.Object);

            var value = _accessor.GetSetting("Key", new SettingsNamespace("NS"), "Setting");
            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        public void ShouldSetValueToStoreOfGivenKey()
        {
            var store = new Mock<ISettingsStore>();
            _accessor.Set("Key", store.Object);

            _accessor.SetSetting("Key", new SettingsNamespace("NS"), "Setting", null);
            store.Verify(s => s.SetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.IsAny<object>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ShouldPassSettingsNamespaceToStoreWhenGettingASetting()
        {
            var store = new Mock<ISettingsStore>();
            var expectedNamespace = new SettingsNamespace("Namespace");
            _accessor.DefaultStore = store.Object;

            _accessor.GetSetting("Key", expectedNamespace, "Setting");
            store.Verify(s => s.GetSetting(It.Is<SettingsNamespace>(ns => expectedNamespace.Equals(ns)), It.IsAny<string>()));
        }

        [TestMethod]
        public void ShouldPassSettingNameToStoreWhenGettingASetting()
        {
            var store = new Mock<ISettingsStore>();
            var expectedName = "Setting";
            _accessor.DefaultStore = store.Object;

            _accessor.GetSetting("Key", new SettingsNamespace("NS"), expectedName);
            store.Verify(s => s.GetSetting(It.IsAny<SettingsNamespace>(), It.Is<string>(name => expectedName == name)));
        }

        [TestMethod]
        public void ShouldPassSettingsNamespaceToStoreWhenSettingASetting()
        {
            var store = new Mock<ISettingsStore>();
            var expectedNamespace = new SettingsNamespace("Namespace");
            _accessor.DefaultStore = store.Object;

            _accessor.SetSetting("Key", expectedNamespace, "Setting", null);
            store.Verify(s => s.SetSetting(It.Is<SettingsNamespace>(ns => expectedNamespace.Equals(ns)), It.IsAny<string>(), It.IsAny<object>()));
        }

        [TestMethod]
        public void ShouldPassSettingNameToStoreWhenSettingASetting()
        {
            var store = new Mock<ISettingsStore>();
            var expectedName = "Setting";
            _accessor.DefaultStore = store.Object;

            _accessor.SetSetting("Key", new SettingsNamespace("NS"), expectedName, null);
            store.Verify(s => s.SetSetting(It.IsAny<SettingsNamespace>(), It.Is<string>(name => expectedName == name), It.IsAny<object>()));
        }

        [TestMethod]
        public void ShouldPassSettingValueToStoreWhenSettingASetting()
        {
            var store = new Mock<ISettingsStore>();
            var expectedValue = new object();
            _accessor.DefaultStore = store.Object;

            _accessor.SetSetting("Key", new SettingsNamespace("NS"), "Name", expectedValue);
            store.Verify(s => s.SetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.Is<object>(value => value == expectedValue)));
        }
    }
}
