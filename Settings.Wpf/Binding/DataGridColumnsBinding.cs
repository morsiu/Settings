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
using TheSettings.Infrastructure.Factories;
using TheSettings.Wpf.Binding.Adapters;
using TheSettings.Wpf.Binding.ValueConverters;

namespace TheSettings.Wpf.Binding
{
    public class DataGridColumnsBinding : ISettingBindingsProvider
    {
        private static readonly KeyedFactoryContainer<string, ColumnSettingNameFactory> _columnSettingNameFactories = new KeyedFactoryContainer<string, ColumnSettingNameFactory>();

        public static readonly string DefaultSettingNameFactoryKey = string.Empty;

        public static readonly List<DependencyProperty> DefaultStoredProperties =
            new List<DependencyProperty>
            {
                DataGridColumn.VisibilityProperty,
                DataGridColumn.DisplayIndexProperty,
                DataGridColumn.SortDirectionProperty,
                DataGridColumn.WidthProperty
            };

        public DataGridColumnsBinding()
        {
            SettingNameFactoryKey = DefaultSettingNameFactoryKey;
        }

        public static void RegisterColumnSettingNameFactory(string name, ColumnSettingNameFactory factory)
        {
            _columnSettingNameFactories.RegisterFactory(name, factory);
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
            var synchronizationGroup = new SynchronizationGroup();
            builder.UseSynchronizationGroup(synchronizationGroup);
            var dataGrid = (DataGrid)target;
            var @namespace = Settings.GetNamespace(target);
            var columns = GetColumnsInInitializationOrder(dataGrid, @namespace);
            var bindings = GetStoredProperties()
                .SelectMany(
                    property => columns.Select((c, i) => new { Column = c, ColumnIndex = dataGrid.Columns.IndexOf(c) }),
                    (property, c) => BindColumn(c.Column, c.ColumnIndex, property, @namespace, builder));
            return bindings;
        }

        private ISettingBinding BindColumn(
            DataGridColumn column,
            int columnIndex,
            DependencyProperty storedProperty,
            SettingsNamespace @namespace,
            ValueBindingBuilder builder)
        {
            var accessor = Settings.CurrentStoreAccessor;
            var settingName = GetSettingName(Setting, column, columnIndex, storedProperty);
            var targetAdapter = CreateTargetAdapter(column, storedProperty);
            var exceptionHandler = new DebugValueAdapterExceptionHandler(storedProperty.Name, column, Store, settingName, @namespace);
            var binding = builder
                .SetTargetAdapter(targetAdapter)
                .SetSourceAdapter(accessor, Store, @namespace, settingName)
                .SetExceptionHandler(exceptionHandler.LogAndSwallowException)
                .Build();
            return binding;
        }

        private IEnumerable<DataGridColumn> GetColumnsInInitializationOrder(DataGrid dataGrid, SettingsNamespace settingsNamespace)
        {
            // When columns' DisplayIndex is used, columns need to be initialized in ascending order of target display index.
            // This way the initialization of bindings of column displayed after previously initialized column won't change the display index of the latter.
            if (GetStoredProperties().Contains(DataGridColumn.DisplayIndexProperty))
            {
                var orderedColumns = (
                    from c in dataGrid.Columns.Select((c, i) => new { Column = c, Index = i })
                    let column = c.Column
                    let index = c.Index
                    let displayIndexSetting =
                        Settings.CurrentStoreAccessor.GetSetting(
                            Store,
                            settingsNamespace,
                            GetSettingName(Setting, column, index, DataGridColumn.DisplayIndexProperty))
                    where displayIndexSetting != SettingsConstants.NoValue
                    orderby (int)displayIndexSetting ascending
                    select column)
                    .ToList();

                orderedColumns.AddRange(dataGrid.Columns.Except(orderedColumns).ToList());
                return orderedColumns;
            }
            return dataGrid.Columns;
        }

        private IEnumerable<DependencyProperty> GetStoredProperties()
        {
            return StoredProperties
                ?? DefaultStoredProperties;
        }

        private string GetSettingName(string settingName, DataGridColumn column, int columnIndex, DependencyProperty storedProperty)
        {
            if (SettingNameFactory != null)
            {
                return SettingNameFactory(settingName, column, columnIndex, storedProperty);
            }
            return _columnSettingNameFactories.CreateValue<string>(SettingNameFactoryKey, factory => factory(settingName, column, columnIndex, storedProperty));
        }

        private static IValueAdapter CreateTargetAdapter(DataGridColumn column, DependencyProperty storedProperty)
        {
            var propertyType = storedProperty.PropertyType;
            var propertyAdapter =  new DependencyPropertyAdapter(column, storedProperty);
            return WrapInTypeConverterIfNecessary(propertyAdapter, propertyType);
        }

        private static IValueAdapter WrapInTypeConverterIfNecessary(IValueAdapter propertyAdapter, Type propertyType)
        {
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
            if (IsApplicableForTypeConversion(propertyType))
            {
                var converter = TypeDescriptor.GetConverter(propertyType);
                return converter;
            }
            return null;
        }

        private static bool IsApplicableForTypeConversion(Type propertyType)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType);
            var isEnum = propertyType.IsEnum || (underlyingType != null && underlyingType.IsEnum);
            var hasTypeConverterAttribute = propertyType.IsDefined(typeof(TypeConverterAttribute), false);
            return isEnum || hasTypeConverterAttribute;
        }
    }
}