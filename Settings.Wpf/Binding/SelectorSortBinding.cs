// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using TheSettings.Binding;
using TheSettings.Binding.ValueAdapters;
using TheSettings.Wpf.Binding.Adapters;

namespace TheSettings.Wpf.Binding
{
    public class SelectorSortBinding : ISettingBindingsProvider
    {
        public object Store { get; set; }

        public string Setting { get; set; }

        // TODO: (HACK) Invent better abstraction
        public Func<ICollectionAdapter, ICollectionAdapter> TargetAdapterBuilder { get; set; }

        public IEnumerable<ISettingBinding> ProvideBindings(DependencyObject target)
        {
            var selector = target as Selector;
            if (selector == null || Setting == null)
            {
                return Enumerable.Empty<ISettingBinding>();
            }

            var targetAdapterBuilder = TargetAdapterBuilder ?? (adapter => adapter);
            var settingAdapter = new SettingAdapter(Settings.CurrentStoreAccessor, Store, Settings.GetNamespace(selector), Setting);
            var binding =
                new ListBinding(
                    targetAdapterBuilder(
                        new SelectorSortDescriptionsAdapter(
                            new SelectorItemsSourceViewAdapter(selector))),
                    settingAdapter);
            return new[] { binding };
        }
    }
}