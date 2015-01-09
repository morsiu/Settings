// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings.Binding.ValueAdapters;
using TheSettingsTests.Mocks;

namespace TheSettingsTests.ValueAdapterTests
{
    [TestClass]
    public class ConvertingAdapterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotCreateWithoutConverter()
        {
            new ConvertingAdapter(new ValueAdapter(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotCreateWithoutSourceAdapter()
        {
            new ConvertingAdapter(null, new ValueConverter());
        }

        [TestMethod]
        public void ShouldForwardSourceValueChangedCallbackCalls()
        {
            var converter = new ValueConverter();
            var sourceAdapter = new ValueAdapter();
            var adapter = new ConvertingAdapter(sourceAdapter, converter);
            bool wasCalled = false;

            adapter.ValueChangedCallback = _ => { wasCalled = true; };
            sourceAdapter.ValueChangedCallback(null);

            Assert.IsTrue(wasCalled);
        }

        [TestMethod]
        public void ShouldForwardConvertedValueFromSourceValueChangedCallback()
        {
            var converter = new ValueConverter { ReturnedSource = 5 };
            var sourceAdapter = new ValueAdapter();
            var adapter = new ConvertingAdapter(sourceAdapter, converter);
            object actualValue = null;

            adapter.ValueChangedCallback = value => { actualValue = value; };
            sourceAdapter.ValueChangedCallback(1);

            Assert.AreEqual(5, actualValue);
        }

        [TestMethod]
        public void ShouldGetValueFromSourceAdapterWhenGettingValue()
        {
            var converter = new NullValueConverter();
            var sourceAdapter = new ValueAdapter { Value = 5 };
            var adapter = new ConvertingAdapter(sourceAdapter, converter);

            object actualValue = adapter.GetValue();

            Assert.AreEqual(5, actualValue);
        }

        [TestMethod]
        public void ShouldSetValueToSourceAdapterWhenSettingValue()
        {
            var converter = new NullValueConverter();
            var sourceAdapter = new ValueAdapter();
            var adapter = new ConvertingAdapter(sourceAdapter, converter);

            adapter.SetValue(5);

            Assert.AreEqual(5, sourceAdapter.Value);
        }

        [TestMethod]
        public void ShouldGetConvertedValueWhenGettingValue()
        {
            var converter = new ValueConverter { ReturnedSource = 5 };
            var sourceAdapter = new ValueAdapter();
            var adapter = new ConvertingAdapter(sourceAdapter, converter);

            var actualValue = adapter.GetValue();

            Assert.AreEqual(5, actualValue);
        }

        [TestMethod]
        public void ShouldSetConvertedValueWhenSettingValue()
        {
            var converter = new ValueConverter { ReturnedTarget = 5 };
            var sourceAdapter = new ValueAdapter();
            var adapter = new ConvertingAdapter(sourceAdapter, converter);

            adapter.SetValue(null);

            Assert.AreEqual(5, sourceAdapter.Value);
        }
    }
}