﻿using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;

namespace Settings.Binding.Wpf
{
    internal static class SettingsMarkupExtensions
    {
        public static T RequireService<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            var service = serviceProvider.GetService(typeof(T)) as T;
            if (service == null)
            {
                throw new InvalidOperationException(string.Format("Failed to retrieve required service of type {0}", typeof(T)));
            }
            return service;
        }

        public static SettingsInitializer RequireInitializer(this IRootObjectProvider rootProvider)
        {
            var root = rootProvider.RootObject as FrameworkElement;
            if (root == null)
            {
                throw new InvalidOperationException("No root object provided or it is not a FrameworkElement.");
            }
            var initializer = SettingsInitializer.GetInstance(root);
            if (initializer == null)
            {
                throw new InvalidOperationException("Failed to obtain initializer.");
            }
            return initializer;
        }

        public static object GetUnsetValue(object targetObject, object targetProperty)
        {
            var clrProperty = targetProperty as PropertyInfo;
            if (clrProperty != null)
            {
                return clrProperty.GetValue(targetObject);
            }
            var dProperty = targetProperty as DependencyProperty;
            var dObject = targetObject as DependencyObject;
            if (dProperty != null && dObject != null)
            {
                return dObject.GetValue(dProperty);
            }
            throw new InvalidOperationException("Cannot retrieve default value from target property of target object as they are not supported.");
        }

        public static Type GetTargetPropertyValueType(this IProvideValueTarget provider)
        {
            var targetProperty = provider.TargetProperty;
            var clrProperty = targetProperty as PropertyInfo;
            if (clrProperty != null)
            {
                return clrProperty.PropertyType;
            }
            var dProperty = targetProperty as DependencyProperty;
            if (dProperty != null)
            {
                return dProperty.PropertyType;
            }
            throw new InvalidOperationException("Cannot retrieve default value from target property of target object as they are not supported.");
        }
    }
}