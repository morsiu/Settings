// Copyright (C) Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using TheSettings.Binding.ValueAdapters;
using TheSettings.Binding.ValueConverters;

namespace TheSettings.Binding
{
    public static class ValueAdapterFactory
    {
        public static IValueAdapter CreateAdapter(object target, object property)
        {
            if (target == null) throw new ArgumentNullException("target");
            return TryCreateAdapterFromPropertyName(target, property)
                ?? TryCreateAdapterFromPropertyInfo(target, property)
                ?? TryCreateAdapterFromDependencyProperty(target, property)
                ?? TryCreateAdapterFromDependencyPropertyDescriptor(target, property)
                ?? TryCreateAdapterFromDescriptor(target, property);
        }

        public static IValueAdapter CreateAdapterFromPropertyName(object target, string propertyName)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            return TryCreateAdapterFromDependencyProperty(target, propertyName)
                ?? TryCreateAdapterFromPropertyInfo(target, propertyName)
                ?? TryCreateAdapterFromDescriptor(target, propertyName);
        }

        private static IValueAdapter ConvertSourceToType(IValueAdapter adapter, Type sourceTargetType)
        {
            return new ConvertingAdapter(adapter, new SourceValueConverter(sourceTargetType));
        }

        public static IValueAdapter CreateAdapterForDependencyProperty(DependencyObject target, DependencyProperty property)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (property == null) throw new ArgumentNullException("property");
            return ConvertSourceToType(
                new DependencyPropertyAdapter(target, property),
                property.PropertyType);
        }

        private static IValueAdapter CreateAdapterForDescriptor(object target, PropertyDescriptor descriptor)
        {
            return ConvertSourceToType(
                new DescriptorAdapter(target, descriptor),
                descriptor.PropertyType);
        }

        private static IValueAdapter CreateAdapterForPropertyInfo(object target, PropertyInfo propertyInfo)
        {
            return target is INotifyPropertyChanged
                ? new ClrPropertyAdapter(target, propertyInfo)
                : TryCreateAdapterFromDescriptor(target, propertyInfo.Name);
        }

        private static IValueAdapter TryCreateAdapterFromDependencyProperty(object target, string propertyName)
        {
            var dependencyTarget = target as DependencyObject;
            if (dependencyTarget == null)
                return null;
            var dependencyPropertyFieldInfo = target.GetType().GetField(propertyName + "Property", BindingFlags.Public | BindingFlags.Static);
            if (dependencyPropertyFieldInfo == null)
                return null;
            var dependencyProperty = dependencyPropertyFieldInfo.GetValue(target) as DependencyProperty;
            if (dependencyProperty == null)
                return null;
            return CreateAdapterForDependencyProperty(dependencyTarget, dependencyProperty);
        }

        private static IValueAdapter TryCreateAdapterFromDependencyProperty(object target, object property)
        {
            var dependencyProperty = property as DependencyProperty;
            var dependencyTarget = target as DependencyObject;
            return dependencyTarget != null && dependencyProperty != null
                ? CreateAdapterForDependencyProperty(dependencyTarget, dependencyProperty)
                : null;
        }

        private static IValueAdapter TryCreateAdapterFromDependencyPropertyDescriptor(object target, object property)
        {
            var dependencyPropertyDescriptor = property as DependencyPropertyDescriptor;
            var dependencyTarget = target as DependencyObject;
            return dependencyTarget != null && dependencyPropertyDescriptor != null
                ? CreateAdapterForDependencyProperty(dependencyTarget, dependencyPropertyDescriptor.DependencyProperty)
                : null;
        }

        private static IValueAdapter TryCreateAdapterFromDescriptor(object target, string propertyName)
        {
            var propertyDescriptor = TypeDescriptor.GetProperties(target)[propertyName];
            return propertyDescriptor != null
                ? CreateAdapterForDescriptor(target, propertyDescriptor)
                : null;
        }

        private static IValueAdapter TryCreateAdapterFromDescriptor(object target, object property)
        {
            var propertyDescriptor = property as PropertyDescriptor;
            return propertyDescriptor != null
                ? CreateAdapterForDescriptor(target, propertyDescriptor)
                : null;
        }

        private static IValueAdapter TryCreateAdapterFromPropertyInfo(object target, string propertyName)
        {
            var propertyInfo = target.GetType().GetProperty(propertyName);
            return propertyInfo != null
                ? CreateAdapterForPropertyInfo(target, propertyInfo)
                : null;
        }

        private static IValueAdapter TryCreateAdapterFromPropertyInfo(object target, object property)
        {
            var propertyInfo = property as PropertyInfo;
            return propertyInfo != null
                ? CreateAdapterForPropertyInfo(target, propertyInfo)
                : null;
        }

        private static IValueAdapter TryCreateAdapterFromPropertyName(object target, object property)
        {
            var propertyName = property as string;
            return propertyName != null
                ? CreateAdapterFromPropertyName(target, propertyName)
                : null;
        }
    }
}