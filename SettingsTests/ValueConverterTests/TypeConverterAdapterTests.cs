// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TheSettings;
using TheSettings.Wpf.Binding.ValueConverters;

namespace TheSettingsTests.ValueConverterTests
{
    [TestClass]
    public class TypeConverterAdapterTests
    {
        private Mock<TypeConverter> _typeConverter;

        [TestInitialize]
        public void TestInitialize()
        {
            _typeConverter = new Mock<TypeConverter>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldThrowGivenNullTypeConverter()
        {
            new TypeConverterAdapter(null, typeof(int));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldThrowGivenNullSourceType()
        {
            SetupTypeConverterToConversion(true);
            new TypeConverterAdapter(_typeConverter.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorShouldThrowWhenTypeConverterDoesNotConvertToSourceType()
        {
            SetupTypeConverterToConversion(false);
            new TypeConverterAdapter(_typeConverter.Object, typeof(int));
        }

        [TestMethod]
        public void ConvertSourceShouldUseTypeConverterConvertFromForSourceConversion()
        {
            SetupTypeConverterToConversion(true);
            SetupTypeConverterFromConversion(true);
            var sourceType = typeof(int);
            var converter = new TypeConverterAdapter(_typeConverter.Object, sourceType);

            converter.ConvertSource(5);

            _typeConverter.Verify(
                c => c.ConvertFrom(
                    It.IsAny<ITypeDescriptorContext>(),
                    It.IsAny<CultureInfo>(),
                    It.IsAny<object>()));
        }

        [TestMethod]
        public void ConvertTargetShouldReturnNoValueWhenTypeConverterConvertToThrows()
        {
            SetupTypeConverterToConversion(true);
            var converter = new TypeConverterAdapter(_typeConverter.Object, typeof(int));
            _typeConverter.Setup(c => c.ConvertTo(It.IsAny<ITypeDescriptorContext>(), It.IsAny<CultureInfo>(), It.IsAny<object>(), It.IsAny<Type>()))
                .Throws<Exception>();

            var source = converter.ConvertTarget(5);

            Assert.AreEqual(SettingsConstants.NoValue, source);
        }

        [TestMethod]
        public void ConvertSourceShouldReturnTypeConverterConvertFromResult()
        {
            SetupTypeConverterToConversion(true);
            SetupTypeConverterFromConversion(true, 10);
            var converter = new TypeConverterAdapter(_typeConverter.Object, typeof(int));

            var target = converter.ConvertSource(5);

            Assert.AreEqual(10, target);
        }

        [TestMethod]
        public void ConvertTargetShouldReturnTypeConverterConvertToResult()
        {
            SetupTypeConverterToConversion(true, 10);
            var converter = new TypeConverterAdapter(_typeConverter.Object, typeof(int));

            var source = converter.ConvertTarget(5);

            Assert.AreEqual(10, source);
        }

        private void SetupTypeConverterToConversion(bool canConvert, object targetValue = null)
        {
            _typeConverter.Setup(c => c.CanConvertTo(It.IsAny<ITypeDescriptorContext>(), It.IsAny<Type>()))
                .Returns(canConvert);
            _typeConverter.Setup(c => c.ConvertTo(It.IsAny<ITypeDescriptorContext>(), It.IsAny<CultureInfo>(), It.IsAny<object>(), It.IsAny<Type>()))
                .Returns(targetValue);
        }

        private void SetupTypeConverterFromConversion(bool canConvert, object sourceValue = null)
        {
            _typeConverter.Setup(c => c.CanConvertFrom(It.IsAny<ITypeDescriptorContext>(), It.IsAny<Type>()))
                .Returns(canConvert);
            _typeConverter.Setup(c => c.ConvertFrom(It.IsAny<ITypeDescriptorContext>(), It.IsAny<CultureInfo>(), It.IsAny<object>()))
                .Returns(sourceValue);
        }
    }
}