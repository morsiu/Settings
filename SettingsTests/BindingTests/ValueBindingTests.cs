// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using TheSettings.Binding;
using TheSettingsTests.Mocks;

namespace TheSettingsTests.BindingTests
{
    [TestClass]
    public class ValueBindingTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutSourceAdapter()
        {
            new ValueBinding(null, new ValueAdapter());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotConstructWithoutTargetAdapter()
        {
            new ValueBinding(new ValueAdapter(), null);
        }

        [TestMethod]
        public void WhenSourceValueChangesItShouldBePassedToTarget()
        {
            var sourceAdapter = new ValueAdapter();
            var targetAdapter = new ValueAdapter();
            var binding = new ValueBinding(targetAdapter, sourceAdapter);

            sourceAdapter.ValueChangedCallback(5);

            Assert.AreEqual(5, targetAdapter.Value);
        }

        [TestMethod]
        public void WhenTargetValueChangesItShouldBePassedToSource()
        {
            var sourceAdapter = new ValueAdapter();
            var targetAdapter = new ValueAdapter();
            var binding = new ValueBinding(targetAdapter, sourceAdapter);

            targetAdapter.ValueChangedCallback(5);

            Assert.AreEqual(5, sourceAdapter.Value);
        }

        [TestMethod]
        public void UpdateTargetShouldPassValueFromSourceToTarget()
        {
            var sourceAdapter = new ValueAdapter();
            var targetAdapter = new ValueAdapter();
            var binding = new ValueBinding(targetAdapter, sourceAdapter);
            sourceAdapter.Value = 5;

            binding.UpdateTarget();

            Assert.AreEqual(5, sourceAdapter.Value);
        }

        [TestMethod]
        public void WhenSourceValueChangedAndIsNoValueThenTargetValueShouldNotBeChanged()
        {
            var sourceAdapter = new ValueAdapter();
            var targetAdapter = new ValueAdapter();
            var binding = new ValueBinding(targetAdapter, sourceAdapter);

            sourceAdapter.ValueChangedCallback(SettingsConstants.NoValue);

            Assert.IsFalse(targetAdapter.SetValueCalled);
        }

        [TestMethod]
        public void UpdateTargetShouldNotPassNoValueFromSourceToTarget()
        {
            var sourceAdapter = new ValueAdapter();
            var targetAdapter = new ValueAdapter();
            var binding = new ValueBinding(targetAdapter, sourceAdapter);
            sourceAdapter.Value = SettingsConstants.NoValue;

            binding.UpdateTarget();

            Assert.IsFalse(targetAdapter.SetValueCalled);
        }
    }
}