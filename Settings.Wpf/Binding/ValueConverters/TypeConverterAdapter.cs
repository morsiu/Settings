// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Globalization;
using TheSettings.Binding;

namespace TheSettings.Wpf.Binding.ValueConverters
{
    /// <summary>
    /// Value converter that uses TypeConverter instance for converting values.
    /// For conversion to source values the TypeConverter.ConvertTo method is used.
    /// For conversion to target values the TypeConverter.ConvertFrom method is used.
    /// </summary>
    public class TypeConverterAdapter : IValueConverter
    {
        private readonly TypeConverter _converter;
        private readonly Type _sourceType;
        private static readonly CultureInfo _culture = CultureInfo.InvariantCulture;

        /// <param name="converter">Converter that is used for converting values.</param>
        /// <param name="sourceType">The desired type of source values.</param>
        public TypeConverterAdapter(TypeConverter converter, Type sourceType)
        {
            if (converter == null) throw new ArgumentNullException("converter");
            if (sourceType == null) throw new ArgumentNullException("sourceType");
            if (!converter.CanConvertTo(sourceType))
            {
                throw new ArgumentException("sourceType", "Provided converter does not support conversion to given sourceType.");
            }
            _converter = converter;
            _sourceType = sourceType;
        }

        public object ConvertSource(object source)
        {
            var canConvertFromSource = _converter.CanConvertFrom(null, source == null ? _sourceType : source.GetType());
            var target = canConvertFromSource
                ? _converter.ConvertFrom(null, _culture, source)
                : SettingsConstants.NoValue;
            return target;
        }

        public object ConvertTarget(object target)
        {
            object source;
            try
            {
                source = _converter.ConvertTo(null, _culture, target, _sourceType);
            }
            catch (Exception)
            {
                source = SettingsConstants.NoValue;
            }
            return source;
        }
    }
}