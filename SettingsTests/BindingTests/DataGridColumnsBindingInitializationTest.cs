// Copyright (C) 2015 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Linq;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using TheSettings.Binding;
using TheSettings.Binding.Accessors;
using TheSettings.Stores;
using TheSettings.Wpf;
using TheSettings.Wpf.Binding;
using TheSettingsTests.Infrastructure;
using TheSettingsTests.MarkupTests.SettingExtensionTests.Windows;

namespace TheSettingsTests.BindingTests
{
    [TestClass]
    public class DataGridColumnsBindingInitializationTest : GuiTests
    {
        private static readonly SettingsNamespace SettingsNamespace = new SettingsNamespace("Namespace");
        private ISettingsStoreAccessor _oldSettingsStoreAccessor;
        private ISettingsStore _settingsStore;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            DataGridColumnsBinding.RegisterColumnSettingNameFactory(
                "DataGridColumnsBindingInitializationTest",
                (settingName, column, columnIndex, storedProperty) => string.Format("Column{0}{1}", column.Header, storedProperty.Name));
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _settingsStore = new SettingsStore();
            _oldSettingsStoreAccessor = Settings.CurrentStoreAccessor;
            Settings.CurrentStoreAccessor = new SingleSettingsStoreAccessor(_settingsStore);
        }

        [TestMethod]
        public void ShouldInitializeColumnBindingsInOrderWhichPreservesColumnsDisplayIndexes()
        {
            _settingsStore.SetSetting(SettingsNamespace, "ColumnDDisplayIndex", 0);
            _settingsStore.SetSetting(SettingsNamespace, "ColumnCDisplayIndex", 1);
            var window = CreateShowThenCloseAtCleanup<DataGridColumnsBindingInitialization>();
            var binding =
                new DataGridColumnsBinding
                {
                    SettingNameFactoryKey = "DataGridColumnsBindingInitializationTest",
                    StoredProperties = new[] { DataGridColumn.DisplayIndexProperty }
                };
            Settings.GetSettings(window.DataGrid).AddBindingProvider(binding);

            var columnOrderByHeader = window.DataGrid.Columns.OrderBy(c => c.DisplayIndex).Select(c => c.Header).ToArray();
            var expectedColumnOrderByHeader = new[] { "D", "C", "A", "B" };
            CollectionAssert.AreEqual(
                expectedColumnOrderByHeader,
                columnOrderByHeader,
                "The columns have wrong display order: " + string.Join(",", columnOrderByHeader)
                    + "; expected order is: " + string.Join(",", expectedColumnOrderByHeader));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Settings.CurrentStoreAccessor = _oldSettingsStoreAccessor;
        }
    }
}