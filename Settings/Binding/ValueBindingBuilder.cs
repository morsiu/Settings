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
    public class ValueBindingBuilder
    {
        private IValueAdapter _targetAdapter;
        private IValueAdapter _sourceAdapter;
        private ExceptionHandlingAdapter.ExceptionHandler _exceptionHandler;

        public ValueBindingBuilder SetTargetAdapter(
            object target,
            object propertyInfo)
        {
            var targetAdapter = GetPropertyAdapter(target, propertyInfo);
            if (targetAdapter == null)
            {
                throw new ArgumentException("Unsupported type of target and property");
            }
            _targetAdapter = targetAdapter;
            return this;
        }

        public ValueBindingBuilder SetTargetAdapter(
            DependencyObject target,
            DependencyProperty property)
        {
            _targetAdapter = new DependencyPropertyAdapter(target, property);
            return this;
        }

        public ValueBindingBuilder SetTargetAdapter(IValueAdapter adapter)
        {
            _targetAdapter = adapter;
            return this;
        }

        public ValueBindingBuilder SetSourceAdapter(
            ISettingsStoreAccessor storeAccessor,
            object storeKey,
            SettingsNamespace @namespace,
            string settingName)
        {
            _sourceAdapter = CreateSettingAdapter(storeAccessor, storeKey, @namespace, settingName);
            return this;
        }

        public ValueBindingBuilder SetExceptionHandler(ExceptionHandlingAdapter.ExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
            return this;
        }

        public ISettingBinding Build()
        {
            var targetAdapter = _targetAdapter;
            if (_exceptionHandler != null)
            {
                targetAdapter = new ExceptionHandlingAdapter(targetAdapter, _exceptionHandler);
            }
            return new ValueBinding(targetAdapter, _sourceAdapter);
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