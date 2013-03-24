using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings.Binding.ValueAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheSettingsTests.ValueAdapterTests
{
    [TestClass]
    public class DependencyPropertyAdapterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullTarget()
        {
            new DependencyPropertyAdapter(null, Entity.SampleProperty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullProperty()
        {
            new DependencyPropertyAdapter(new Entity(), null);
        }

        [TestMethod]
        public void ShouldGetValue()
        {
            var entity = new Entity { Sample = 5 };
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            var value = adapter.GetValue();
            Assert.AreEqual(5, value);
        }

        [TestMethod]
        public void ShouldSetValue()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            adapter.SetValue(5);
            Assert.AreEqual(5, entity.Sample);
        }

        [TestMethod]
        public void ShouldNotifyAboutValueChanges()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            var callbackCallCount = 0;
            adapter.ValueChangedCallback = newValue => ++callbackCallCount;
            entity.Sample = 3;
            Assert.AreEqual(1, callbackCallCount);
        }

        [TestMethod]
        public void ShouldGiveNewValueToValueChangedCallback()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            object value = null;
            adapter.ValueChangedCallback = newValue => value = newValue;
            entity.Sample = 5;
            Assert.AreEqual(5, value);
        }

        [TestMethod]
        public void ShouldNotFailWhenSettingValueAndValueChangedCallbackIsNotSet()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            try
            {
                entity.Sample = 2;
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ShouldNotNotifyOldValueChangedCallbackWhenValueHasChangedAndNewIsSet()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            var callbackCallCount = 0;
            adapter.ValueChangedCallback = newValue => ++callbackCallCount;
            entity.Sample = 3;
            adapter.ValueChangedCallback = newValue => { };
            entity.Sample = 4;
            Assert.AreEqual(1, callbackCallCount);
        }

        [TestMethod]
        public void ShouldNotCallNotifyValueChangedCallbackWhenValueChangesAfterBeingDisposed()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            var callbackCallCount = 0;
            adapter.ValueChangedCallback = newValue => ++callbackCallCount;
            adapter.Dispose();
            entity.Sample = 3;
            Assert.AreEqual(0, callbackCallCount);
        }

        [TestMethod]
        public void ShouldDoNothingWhenSettingValueToReadOnlyProperty()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleReadOnlyProperty);
            try
            {
                adapter.SetValue(5);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ShouldDoNothingAfterValueChangedCallbackIsSetToNullWhenValueChanges()
        {
            var entity = new Entity();
            var adapter = new DependencyPropertyAdapter(entity, Entity.SampleProperty);
            adapter.ValueChangedCallback = newValue => { };
            adapter.ValueChangedCallback = null;
            try
            {
                entity.Sample = 3;
            }
            catch
            {
                Assert.Fail();
            }
        }

        private class Entity : DependencyObject
        {
            public static readonly DependencyProperty SampleProperty = 
                DependencyProperty.Register(
                    "Sample",
                    typeof(int),
                    typeof(Entity));

            public static readonly DependencyProperty SampleReadOnlyProperty;

            private static readonly DependencyPropertyKey SampleReadOnlyPropertyKey;

            static Entity()
            {
                SampleReadOnlyPropertyKey =
                    DependencyProperty.RegisterReadOnly(
                        "SampleReadOnly",
                        typeof(int),
                        typeof(Entity),
                        new PropertyMetadata());
                SampleReadOnlyProperty = SampleReadOnlyPropertyKey.DependencyProperty;
            }

            public int Sample
            {
                get { return (int)GetValue(SampleProperty); }
                set { SetValue(SampleProperty, value); }
            }

            public int SampleReadOnly
            {
                get { return (int)GetValue(SampleReadOnlyProperty); }
            }
        }
    }
}
