// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Windows;
using System.Windows.Markup;
using TheSettings.Wpf.Initialization;

namespace TheSettings.Wpf.Markup
{
    public class SettingExtension : MarkupExtension
    {
        private static readonly object UnsetValue = new object();

        public SettingExtension(string name)
        {
            Name = name;
            DefaultValue = UnsetValue;
        }

        public string Name { get; set; }

        public object DefaultValue { get; set; }

        public object Store { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = serviceProvider.RequireService<IProvideValueTarget>();
            if (targetProvider.IsInsideTemplate()
                || targetProvider.IsInsideStyleSetter())
            {
                return this;
            }
            var target = targetProvider.TargetObject as DependencyObject;
            if (target == null)
            {
                throw new InvalidOperationException("Target object is not a DependencyObject.");
            }
            var initializer = serviceProvider.RequireInitializer();
            var builder = new SettingBindingBuilder(target, targetProvider.TargetProperty, Store, Name);
            initializer.QueueSettingBindingBuild(builder);

            if (DefaultValue == UnsetValue)
            {
                var unsetValue = SettingsMarkupExtensions.GetUnsetValue(target, targetProvider.TargetProperty);
                return unsetValue;
            }
            var type = targetProvider.GetTargetPropertyValueType();
            return Convert.ChangeType(DefaultValue, type);
        }
    }
}
