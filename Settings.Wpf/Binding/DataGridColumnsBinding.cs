// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TheSettings.Binding;
using TheSettings.Binding.ValueAdapters;
using TheSettings.Wpf.Binding.ValueConverters;

namespace TheSettings.Wpf.Binding
{
    public class DataGridColumnsBinding : ISettingBindingsProvider
    {
        public static readonly IDictionary<string, ColumnSettingNameFactory> ColumnSettingNameFactories = new Dictionary<string, ColumnSettingNameFactory>();

        private static readonly IEnumerable<DependencyProperty> StoredProperties =
            new[]
            {
                DataGridColumn.DisplayIndexProperty,
                DataGridColumn.SortDirectionProperty,
                DataGridColumn.WidthProperty,
                DataGridColumn.VisibilityProperty
            };

        public static readonly string DefaultColumnSettingNaming = string.Empty;

        public DataGridColumnsBinding()
        {
            ColumnSettingNaming = DefaultColumnSettingNaming;
        }

        public delegate string ColumnSettingNameFactory(DataGridColumn column, int columnIndex, DependencyProperty property);

        public string ColumnSettingNaming { get; set; }

        public object Store { get; set; }

        public IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target)
        {
            var builder = new ValueBindingBuilder();
            var dataGrid = (DataGrid)target;
            var @namespace = Settings.GetNamespace(target);
            var bindings = dataGrid.Columns.SelectMany(
                (column, index) => BindColumn(column, index, @namespace, builder));
            return bindings;
        }

        private IEnumerable<ISettingBinding> BindColumn(
            DataGridColumn column,
            int index,
            SettingsNamespace @namespace,
            ValueBindingBuilder builder)
        {
            var nameFactory = GetColumnSettingNameFactory(ColumnSettingNaming);
            if (nameFactory == null)
            {
                return Enumerable.Empty<ISettingBinding>();
            }
            var accessor = Settings.CurrentStoreAccessor;
            return
                from storedProperty in StoredProperties
                let name = nameFactory(column, index, storedProperty)
                let targetAdapter = CreateTargetAdapter(column, storedProperty)
                let binding = builder
                    .SetTargetAdapter(targetAdapter)
                    .SetSourceAdapter(accessor, Store, @namespace, name)
                    .Build()
                select binding;
        }

        private static ColumnSettingNameFactory GetColumnSettingNameFactory(string factoryName)
        {
            if (factoryName == null)
            {
                return null;
            }
            ColumnSettingNameFactory factory;
            ColumnSettingNameFactories.TryGetValue(factoryName, out factory);
            return factory;
        }

        private static IValueAdapter CreateTargetAdapter(DataGridColumn column, DependencyProperty storedProperty)
        {
            var propertyType = storedProperty.PropertyType;
            var propertyAdapter = new DependencyPropertyAdapter(column, storedProperty);
            var typeConverter = GetTypeConverterIfAny(propertyType);
            if (typeConverter != null)
            {
                var valueConverter = new TypeConverterAdapter(typeConverter, typeof(string));
                return new ConvertingAdapter(propertyAdapter, valueConverter);
            }
            return propertyAdapter;
        }

        private static TypeConverter GetTypeConverterIfAny(Type propertyType)
        {
            if (!propertyType.IsDefined(typeof(TypeConverterAttribute), false))
            {
                return null;
            }
            var converterAttribute = propertyType.GetCustomAttributes(typeof(TypeConverterAttribute), false).OfType<TypeConverterAttribute>().First();
            var converterType = Type.GetType(converterAttribute.ConverterTypeName, false);
            if (converterType == null || !typeof(TypeConverter).IsAssignableFrom(converterType))
            {
                return null;
            }
            return (TypeConverter)Activator.CreateInstance(converterType);
        }
    }
}