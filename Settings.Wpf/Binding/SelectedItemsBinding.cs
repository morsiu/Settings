﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TheSettings.Binding;
using TheSettings.Binding.ValueAdapters;

namespace TheSettings.Wpf.Binding
{
    public class SelectedItemsBinding : ISettingBindingsProvider
    {
        public static readonly IList<CollectionAdapterFactory> AdapterFactories = new List<CollectionAdapterFactory>();

        public SelectedItemsBinding()
        {
        }

        public SelectedItemsBinding(string itemKeyProperty)
        {
            ItemKeyProperty = itemKeyProperty;
        }

        public delegate ICollectionAdapter CollectionAdapterFactory(DependencyObject target, Func<object, object> keySelector);

        public string ItemKeyProperty { get; set; }

        public string Setting { get; set; }

        public object Store { get; set; }

        public IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target)
        {
            Func<object, object> keySelector = item => SelectKey(item, ItemKeyProperty);
            var adapter = AdapterFactories
                .Select(factory => factory(target, keySelector))
                .FirstOrDefault();
            if (adapter == null)
            {
                return Enumerable.Empty<ISettingBinding>();
            }

            var @namespace = Settings.GetNamespace(target);
            var settingsAdapter = new SettingAdapter(Settings.CurrentStoreAccessor, Store, @namespace, Setting);
            var comparer = EqualityComparer<object>.Default;
            return new[] { new SetBinding(adapter, settingsAdapter, comparer) };
        }

        private static object SelectKey(object value, string keyProperty)
        {
            if (value == null)
            {
                return null;
            }
            var propertyInfo = value.GetType().GetProperty(keyProperty);
            if (propertyInfo == null || !propertyInfo.CanRead)
            {
                throw new InvalidOperationException(string.Format("Object of type {0} does not have readable property {1}.", value.GetType(), keyProperty));
            }
            return propertyInfo.GetValue(value);
        }
    }
}