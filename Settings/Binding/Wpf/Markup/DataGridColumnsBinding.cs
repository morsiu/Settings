// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TheSettings.Binding.Wpf.Markup
{
    public class DataGridColumnsBinding : ISettingBindingsProvider
    {
        private const string DefaultColumnSettingFormat = "Column{0}{1}";
        private static readonly IEnumerable<DependencyProperty> StoredProperties =
            new[]
            {
                DataGridColumn.DisplayIndexProperty,
                DataGridColumn.SortDirectionProperty,
                DataGridColumn.WidthProperty,
                DataGridColumn.VisibilityProperty
            };

        public DataGridColumnsBinding()
        {
            ColumnSettingFormat = DefaultColumnSettingFormat;
        }

        public string ColumnSettingFormat { get; set; }

        public object Store { get; set; }

        public IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target)
        {
            var factory = new SettingBindingFactory();
            var dataGrid = (DataGrid)target;
            var @namespace = Settings.GetNamespace(target);
            var bindings = dataGrid.Columns.SelectMany(
                (column, index) => BindColumn(column, index, @namespace, factory));
            return bindings;
        }

        private IEnumerable<ISettingBinding> BindColumn(
            DataGridColumn column, 
            int index, 
            SettingsNamespace @namespace, 
            SettingBindingFactory factory)
        {
            return
                from storedProperty in StoredProperties
                let name = string.Format(ColumnSettingFormat, index, storedProperty.Name)
                let binding = factory.Create(column, storedProperty, Store, @namespace, name)
                select binding;
        }
    }
}
