using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using WpfApplication.Settings.Binding.ValueAdapters;

namespace WpfApplication.Settings.Binding
{
    public class SettingBindingFactory
    {
        public ISettingBinding Create(object target, object propertyInfo, SettingsNamespace @namespace, string settingName)
        {
            var settingAdapter = CreateSettingAdapter(@namespace, settingName);
            var targetAdapter = GetPropertyAdapter(target, propertyInfo);
            if (targetAdapter == null)
            {
                throw new ArgumentException("Unsupported type of target and property");
            }

            return new SettingBinding(targetAdapter, settingAdapter);
        }

        private static SettingAdapter CreateSettingAdapter(SettingsNamespace @namespace, string settingName)
        {
            var store = SettingsStore.Instance;
            return new SettingAdapter(store, @namespace, settingName);
        }

        public ISettingBinding Create(DependencyObject target, DependencyProperty property, SettingsNamespace @namespace, string settingName)
        {
            var settingAdapter = CreateSettingAdapter(@namespace, settingName);
            var targetAdapter = new DependencyPropertyAdapter(target, property);
            return new SettingBinding(targetAdapter, settingAdapter);
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
