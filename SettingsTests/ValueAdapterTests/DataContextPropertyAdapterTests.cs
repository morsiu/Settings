// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using TheSettings.Wpf.Binding.Adapters;

namespace TheSettingsTests.ValueAdapterTests
{
    [TestClass]
    public class DataContextPropertyAdapterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotCreateWithoutTargetObject()
        {
            new DataContextPropertyAdapter(null, "Property");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldNotCreateWithoutProperty()
        {
            new DataContextPropertyAdapter(new FrameworkElement(), null);
        }

        [TestMethod]
        public void ShouldGetValueFromDataContextProperty()
        {
            var target = new FrameworkElement();
            target.DataContext = new DataContext { Property = "PropertyValue" };
            var adapter = new DataContextPropertyAdapter(target, "Property");

            var value = adapter.GetValue();

            Assert.AreEqual("PropertyValue", value);
        }

        [TestMethod]
        public void ShouldSetValueToDataContextProperty()
        {
            var dataContext = new DataContext { Property = "OldPropertyValue" };
            var target = new FrameworkElement();
            target.DataContext = dataContext;
            var adapter = new DataContextPropertyAdapter(target, "Property");

            adapter.SetValue("NewPropertyValue");

            Assert.AreEqual("NewPropertyValue", dataContext.Property);
        }

        [TestMethod]
        public void ShouldCallValueChangedCallbackWhenDataContextPropertyChanges()
        {
            var dataContext = new NotifyingDataContext { Property = "OldPropertyValue" };
            var target = new FrameworkElement();
            target.DataContext = dataContext;
            var adapter = new DataContextPropertyAdapter(target, "Property");
            bool callbackWasCalled = false;
            object newDataContextPropertyValue = null;
            adapter.ValueChangedCallback = 
                newValue =>
                {
                    callbackWasCalled = true;
                    newDataContextPropertyValue = newValue;
                };

            dataContext.Property = "NewPropertyValue";

            Assert.IsTrue(callbackWasCalled);
            Assert.AreEqual("NewPropertyValue", newDataContextPropertyValue);
        }

        [TestMethod]
        public void ShouldCallValueChangedCallbackWhenDataContextChanges()
        {
            var dataContext = new NotifyingDataContext { Property = "OldPropertyValue" };
            var target = new FrameworkElement();
            target.DataContext = dataContext;
            var adapter = new DataContextPropertyAdapter(target, "Property");
            bool callbackWasCalled = false;
            object newDataContextPropertyValue = null;
            adapter.ValueChangedCallback = 
                newValue =>
                {
                    callbackWasCalled = true;
                    newDataContextPropertyValue = newValue;
                };

            target.DataContext = new DataContext { Property = "OtherPropertyValue" };

            Assert.IsTrue(callbackWasCalled);
            Assert.AreEqual("OtherPropertyValue", newDataContextPropertyValue);
        }

        [TestMethod]
        public void ShouldGetValueFromPropertyOfNewDataContextAfterItChanges()
        {
            var target = new FrameworkElement();
            target.DataContext = new DataContext { Property = "PropertyValue" };
            var adapter = new DataContextPropertyAdapter(target, "Property");

            target.DataContext = new DataContext { Property = "OtherPropertyValue" };
            var value = adapter.GetValue();

            Assert.AreEqual("OtherPropertyValue", value);
        }

        [TestMethod]
        public void ShouldSetValueToPropertyOfNewDataContextAfterItChanges()
        {
            var target = new FrameworkElement();
            target.DataContext = new DataContext { Property = "PropertyValue" };
            var adapter = new DataContextPropertyAdapter(target, "Property");

            var newDataContext = new DataContext { Property = "OtherPropertyValue" };
            target.DataContext = newDataContext;
            adapter.SetValue("AnotherPropertyValue");

            Assert.AreEqual("AnotherPropertyValue", newDataContext.Property);
        }

        [TestMethod]
        public void ShouldNotSetValueToPropertyOfOldDataContextAfterItChanges()
        {
            var target = new FrameworkElement();
            var dataContext = new DataContext { Property = "PropertyValue" };
            target.DataContext = dataContext;
            var adapter = new DataContextPropertyAdapter(target, "Property");

            target.DataContext = new DataContext { Property = "OtherPropertyValue" };
            adapter.SetValue("AnotherPropertyValue");

            Assert.AreEqual("PropertyValue", dataContext.Property);
        }

        [TestMethod]
        public void ShouldGetNoValueWhenDataContextDoesNotHaveProperty()
        {
            var target = new FrameworkElement();
            var dataContext = new object();
            target.DataContext = dataContext;
            var adapter = new DataContextPropertyAdapter(target, "Property");

            var value = adapter.GetValue();

            Assert.AreSame(SettingsConstants.NoValue, value);
        }

        [TestMethod]
        public void ShouldGetNoValueWhenDataContextIsNull()
        {
            var target = new FrameworkElement();
            target.DataContext = null;
            var adapter = new DataContextPropertyAdapter(target, "Property");

            var value = adapter.GetValue();

            Assert.AreSame(SettingsConstants.NoValue, value);
        }

        [TestMethod]
        public void ShouldDoNothingWhenSettingPropertyToNullDataContext()
        {
            var target = new FrameworkElement();
            target.DataContext = null;
            var adapter = new DataContextPropertyAdapter(target, "Property");

            adapter.SetValue("AnotherPropertyValue");
        }

        private class DataContext
        {
            public string Property { get; set; }
        }

        private sealed class NotifyingDataContext : INotifyPropertyChanged
        {
            private string _property;

            public event PropertyChangedEventHandler PropertyChanged;

            public string Property
            {
                get { return _property; }
                set 
                {
                    _property = value;
                    OnPropertyChanged();
                }
            }

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}