// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using TheSettings.Binding.ValueAdapters;

namespace TheSettings.Wpf.Binding.Adapters
{
    internal class DebugValueAdapterExceptionHandler
    {
        private const string ContextFormat = "The target property is `{0}`, of object `{1}` ({2}). The setting is `{3}`, with namespace `{4}` and store with key `{5}`";

        private static readonly IDictionary<ValueAdapterAction, string> ActionNames =
            new Dictionary<ValueAdapterAction, string>
            {
                { ValueAdapterAction.GetValue, "getting" },
                { ValueAdapterAction.SetValue, "setting" }
            };

        private readonly string _context;

        public DebugValueAdapterExceptionHandler(string propertyName, object target, object settingsStoreKey, string settingName, SettingsNamespace settingsNamespace)
        {
            _context = string.Format(
                ContextFormat,
                propertyName,
                target,
                target == null ? "no type" : target.GetType().FullName,
                settingName,
                settingsNamespace,
                settingsStoreKey);
        }

        public ExceptionHandlerResult LogAndSwallowException(
            ValueAdapterAction action,
            Exception exception)
        {
            WriteExceptionToDebug(action, exception);
            return ExceptionHandlerResult.SwallowException;
        }

        private void WriteExceptionToDebug(ValueAdapterAction action, Exception exception)
        {
            Debug.WriteLine(
                string.Format(
                    "Exception {0} occured while {1} value from/to setting adapter. {2}.",
                    exception.GetType().FullName,
                    ActionNames[action],
                    _context));
        }
    }
}