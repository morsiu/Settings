// Copyright (C) Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings.Binding;
using TheSettingsTests.Infrastructure;

namespace TheSettingsTests.BindingTests
{
    [TestClass]
    public class ValueAdapterFactoryTests
    {
        [TestMethod]
        public void ReturnedAdapterShouldSetValueOfCompatibleTypeGivenTargetWithNameOfDependencyProperty()
        {
            var target = new DependencyObjectTarget();
            var adapter = ValueAdapterFactory.CreateAdapter(target, "Value");

            adapter.SetValue(4.5d);

            Assert.AreEqual(4, target.Value);
        }

        [TestMethod]
        public void ReturnedAdapterShouldSetValueOfCompatibleTypeGivenTargetWithPropertyDescriptor()
        {
            var target = new Target();
            var propertyDescriptor = TypeDescriptor.GetProperties(target)["Value"];
            var adapter = ValueAdapterFactory.CreateAdapter(target, propertyDescriptor);

            adapter.SetValue(4.5d);

            Assert.AreEqual(4, target.Value);
        }

        [TestMethod]
        public void ReturnedAdapterShouldSetValueOfCompatibleTypeGivenTargetWithPropertyInfo()
        {
            var target = new Target();
            var propertyInfo = target.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
            var adapter = ValueAdapterFactory.CreateAdapter(target, propertyInfo);

            adapter.SetValue(4.5d);

            Assert.AreEqual(4, target.Value);
        }

        [TestMethod]
        public void ReturnedAdapterShouldNotLeakTargetGivenPropertyOfNotifyingTarget()
        {
            AssertLifetime.UnreachableInstanceIsNotLeaked(
                () => new NotifyingTarget(),
                target => ValueAdapterFactory.CreateAdapterFromPropertyName(target, "Value"));
        }

        [TestMethod]
        public void ReturnedAdapterShouldNotLeakTargetGivenTargetWithDependencyProperty()
        {
            AssertLifetime.UnreachableInstanceIsNotLeaked(
                () => new DependencyObjectTarget(),
                target => ValueAdapterFactory.CreateAdapterFromPropertyName(target, "Value"));
        }

        [TestMethod]
        public void ReturnedAdapterShouldNotLeakTargetGivenTargetWithDependencyPropertyInheritedFromBaseClass()
        {
            AssertLifetime.UnreachableInstanceIsNotLeaked(
                () => new Control(),
                target => ValueAdapterFactory.CreateAdapterFromPropertyName(target, "DataContext"));
        }

        [TestMethod]
        public void ShouldReturnNullForNonExistingProperty()
        {
            var entity = new NotifyingTarget();
            var adapter = ValueAdapterFactory.CreateAdapterFromPropertyName(entity, "NonExistingProperty");

            Assert.IsNull(adapter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowGivenNullPropertyName()
        {
            var adapter = ValueAdapterFactory.CreateAdapterFromPropertyName(new NotifyingTarget(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowGivenNullTargetWhenCreatingFromPropertyName()
        {
            var adapter = ValueAdapterFactory.CreateAdapterFromPropertyName(null, "Value");
        }

        private sealed class DependencyObjectTarget : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(DependencyObjectTarget));

            public int Value
            {
                get { return (int)GetValue(ValueProperty); }
                set { SetValue(ValueProperty, value); }
            }
        }

        private sealed class NotifyingTarget : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public int Value { get; set; }
        }

        private sealed class Target
        {
            public int Value { get; set; }
        }
    }
}