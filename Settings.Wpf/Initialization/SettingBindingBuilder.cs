﻿// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Windows;
using TheSettings.Binding;

namespace TheSettings.Wpf.Initialization
{
    internal class SettingBindingBuilder
    {
        private readonly DependencyObject _target;
        private readonly object _property;
        private readonly string _name;
        private readonly object _storeKey;

        public SettingBindingBuilder(DependencyObject target, object property, object storeKey, string name)
        {
            _target = target;
            _property = property;
            _name = name;
            _storeKey = storeKey;
        }

        public void Build()
        {
            var @namespace = Settings.GetNamespace(_target);
            var settings = Settings.GetSettings(_target);
            var bindingBuilder = new ValueBindingBuilder();
            var accessor = Settings.CurrentStoreAccessor;
            var binding = bindingBuilder
                .SetTargetAdapter(_target, _property)
                .SetSourceAdapter(accessor, _storeKey, @namespace, _name)
                .Build();
            settings.AddBinding(binding);
        }
    }
}