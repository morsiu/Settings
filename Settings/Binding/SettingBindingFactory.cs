// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using TheSettings.Binding.ValueAdapters;

namespace TheSettings.Binding
{
    public class SettingBindingFactory
    {
        public ISettingBinding Create(
            object target,
            object propertyInfo,
            ISettingsStoreAccessor storeAccessor,
            object storeKey,
            SettingsNamespace @namespace,
            string settingName)
        {
            var settingAdapter = CreateSettingAdapter(storeAccessor, storeKey, @namespace, settingName);
            var targetAdapter = GetPropertyAdapter(target, propertyInfo);
            if (targetAdapter == null)
            {
                throw new ArgumentException("Unsupported type of target and property");
            }

            return new SettingBinding(targetAdapter, settingAdapter);
        }

        public ISettingBinding Create(
            DependencyObject target,
            DependencyProperty property,
            ISettingsStoreAccessor storeAccessor,
            object storeKey,
            SettingsNamespace @namespace,
            string settingName)
        {
            var settingAdapter = CreateSettingAdapter(storeAccessor, storeKey, @namespace, settingName);
            var targetAdapter = new DependencyPropertyAdapter(target, property);
            return new SettingBinding(targetAdapter, settingAdapter);
        }

        private static SettingAdapter CreateSettingAdapter(ISettingsStoreAccessor storeAccessor, object storeKey, SettingsNamespace @namespace, string settingName)
        {
            return new SettingAdapter(storeAccessor, storeKey, @namespace, settingName);
        }

        private static IValueAdapter GetPropertyAdapter(object target, object propertyInfo)
        {
            return TryDependencyProperty(target, propertyInfo)
                ?? TryClrProperty(target, propertyInfo)
                ?? TryDescriptorAdapter(target, propertyInfo);
        }

        private static IValueAdapter TryDescriptorAdapter(object target, object propertyInfo)
        {
            var descriptor = propertyInfo as PropertyDescriptor;
            if (descriptor != null)
            {
                return new DescriptorAdapter(target, descriptor);
            }
            return null;
        }

        private static IValueAdapter TryClrProperty(object target, object propertyInfo)
        {
            var clrProperty = propertyInfo as PropertyInfo;
            if (clrProperty != null)
            {
                return new ClrPropertyAdapter(target, clrProperty);
            }
            return null;
        }

        private static IValueAdapter TryDependencyProperty(object target, object propertyInfo)
        {
            var dependencyTarget = target as DependencyObject;
            var dependencyProperty = propertyInfo as DependencyProperty;
            if (dependencyTarget != null && dependencyProperty != null)
            {
                return new DependencyPropertyAdapter(dependencyTarget, dependencyProperty);
            }
            return null;
        }
    }
}
