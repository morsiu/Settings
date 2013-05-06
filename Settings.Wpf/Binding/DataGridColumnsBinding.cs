﻿// Copyright (C) 2013 Łukasz Mrozek
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

        public static readonly string DefaultSettingNameFactoryKey = string.Empty;

        public static readonly List<DependencyProperty> DefaultStoredProperties =
            new List<DependencyProperty>
            {
                DataGridColumn.DisplayIndexProperty,
                DataGridColumn.SortDirectionProperty,
                DataGridColumn.WidthProperty,
                DataGridColumn.VisibilityProperty
            };

        public DataGridColumnsBinding()
        {
            SettingNameFactoryKey = DefaultSettingNameFactoryKey;
        }

        public delegate string ColumnSettingNameFactory(string settingName, DataGridColumn column, int columnIndex, DependencyProperty property);

        public ColumnSettingNameFactory SettingNameFactory { get; set; }

        public IEnumerable<DependencyProperty> StoredProperties { get; set; }

        public string SettingNameFactoryKey { get; set; }

        public string Setting { get; set; }

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
            var nameFactory = GetColumnSettingNameFactory();
            if (nameFactory == null)
            {
                return Enumerable.Empty<ISettingBinding>();
            }
            var accessor = Settings.CurrentStoreAccessor;
            return
                from storedProperty in GetStoredProperties()
                let name = nameFactory(Setting, column, index, storedProperty)
                let targetAdapter = CreateTargetAdapter(column, storedProperty)
                let binding = builder
                    .SetTargetAdapter(targetAdapter)
                    .SetSourceAdapter(accessor, Store, @namespace, name)
                    .Build()
                select binding;
        }

        private IEnumerable<DependencyProperty> GetStoredProperties()
        {
            return StoredProperties
                ?? DefaultStoredProperties
                ?? Enumerable.Empty<DependencyProperty>();
        }

        private ColumnSettingNameFactory GetColumnSettingNameFactory()
        {
            if (SettingNameFactory != null)
            {
                return SettingNameFactory;
            }
            if (SettingNameFactoryKey == null)
            {
                return null;
            }
            ColumnSettingNameFactory factory;
            ColumnSettingNameFactories.TryGetValue(SettingNameFactoryKey, out factory);
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