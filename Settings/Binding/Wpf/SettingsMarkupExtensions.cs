using System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Xaml;
using TheSettings.Binding.Wpf.Infrastructure;

namespace TheSettings.Binding.Wpf
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

        public static SettingsInitializer RequireInitializer(this IServiceProvider provider)
        {
            var rootProvider = provider.GetService(typeof(IRootObjectProvider)) as IRootObjectProvider;
            FrameworkElement root = null;
            if (rootProvider != null)
            {
                root = rootProvider.RootObject as FrameworkElement;
            }
            else
            {
                var targetProvider = provider.RequireService<IProvideValueTarget>();
                var target = targetProvider.TargetObject as DependencyObject;
                var treeWalker = new WpfLogicalTreeWalker(target);
                root = treeWalker.ClimbToRoot().CurrentAs<FrameworkElement>();
            }
            if (root == null)
            {
                throw new InvalidOperationException("Cannot determine root object for SettingsInitializer.");
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

        public static bool IsInsideTemplate(this IProvideValueTarget provider)
        {
            return provider.TargetObject.GetType().FullName == "System.Windows.SharedDp";
        }

        public static bool IsInsideStyleSetter(this IProvideValueTarget provider)
        {
            return provider.TargetObject.GetType() == typeof(Setter);
        }
    }
}
