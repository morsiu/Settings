// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheSettings;
using TheSettings.Stores;

namespace TheSettingsTests.StoreTests
{
    [TestClass]
    public class InvalidatableSettingsStoreTests : DelegatingStoreTestSuite
    {
        private const string SettingName = "Setting";
        private static readonly SettingsNamespace Namespace = new SettingsNamespace("NS");

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenCreatedWithNullInvalidator()
        {
            new InvalidatableSettingsStore(new SettingsStore(), null);
        }

        [TestMethod]
        public void ShouldUseInitialStoreWhenGettingASetting()
        {
            var innerStore = new Mock<ISettingsStore>();
            var store = new InvalidatableSettingsStore(innerStore.Object, oldStore => oldStore);

            store.GetSetting(Namespace, SettingName);
            innerStore.Verify(
                s => s.GetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>()),
                Times.AtLeastOnce());
        }

        [TestMethod]
        public void ShouldUseInitialStoreWhenSettingASetting()
        {
            var innerStore = new Mock<ISettingsStore>();
            var store = new InvalidatableSettingsStore(innerStore.Object, oldStore => oldStore);

            store.SetSetting(Namespace, SettingName, null);
            innerStore.Verify(
                s => s.SetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.IsAny<object>()),
                Times.AtLeastOnce());
        }

        [TestMethod]
        public void ShouldUseStoreFromInvalidatorAfterInvalidationWhenGettingASetting()
        {
            var innerStore = new Mock<ISettingsStore>();
            var store = new InvalidatableSettingsStore(null, oldStore => innerStore.Object);

            store.Invalidate();
            store.GetSetting(Namespace, SettingName);

            innerStore.Verify(
                s => s.GetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>()),
                Times.AtLeastOnce());
        }

        [TestMethod]
        public void ShouldUseStoreFromInvalidatorAfterInvalidationWhenSettingASetting()
        {
            var innerStore = new Mock<ISettingsStore>();
            var store = new InvalidatableSettingsStore(null, oldStore => innerStore.Object);

            store.Invalidate();
            store.SetSetting(Namespace, SettingName, null);

            innerStore.Verify(
                s => s.SetSetting(It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.IsAny<object>()),
                Times.AtLeastOnce());
        }

        protected override ISettingsStore ProvideStore(ISettingsStore innerStore)
        {
            return new InvalidatableSettingsStore(innerStore, oldStore => oldStore);
        }
    }
}