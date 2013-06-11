// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace TheSettings
{
    /// <summary>
    /// Represents storage for settings.
    /// </summary>
    public interface ISettingsStore
    {
        /// <summary>
        /// Gets value of setting with given name placed in given namespace.
        /// </summary>
        /// <param name="namespace">Setting namespace.</param>
        /// <param name="name">Setting name.</param>
        /// <returns>Returns value of the setting, if exists in store, otherwise SettingContants.NoValue.</returns>
        object GetSetting(SettingsNamespace @namespace, string name);

        /// <summary>
        /// Sets value of setting with givan name placed in given namespace.
        /// </summary>
        /// <param name="namespace">Setting namespace.</param>
        /// <param name="name">Setting name.</param>
        /// <param name="value">New setting value.</param>
        void SetSetting(SettingsNamespace @namespace, string name, object value);
    }
}
