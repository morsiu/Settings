using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using TheSettings.Binding.ValueAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheSettingsTests.ValueAdapterTests
{
    [TestClass]
    public class ClrPropertyAdapterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullTarget()
        {
            new ClrPropertyAdapter(null, Entity.PropertyInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullPropertyInfo()
        {
            new ClrPropertyAdapter(new Entity(), null);
        }

        [TestMethod]
        public void ShouldGetValue()
        {
            var entity = new Entity { Property = 5 };
            var adapter = new ClrPropertyAdapter(entity, Entity.PropertyInfo);
            var value = adapter.GetValue();
            Assert.AreEqual(5, value);
        }

        [TestMethod]
        public void ShouldSetValue()
        {
            var entity = new Entity();
            var adapter = new ClrPropertyAdapter(entity, Entity.PropertyInfo);
            adapter.SetValue(5);
            Assert.AreEqual(5, entity.Property);
        }

        [TestMethod]
        public void ShouldDoNothingWhenSettingReadOnlyProperty()
        {
            var entity = new Entity();
            var adapter = new ClrPropertyAdapter(entity, Entity.ReadOnlyPropertyInfo);
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
        public void ShouldReturNoValueWhenReadingWriteOnlyProperty()
        {
            var entity = new Entity();
            var adapter = new ClrPropertyAdapter(entity, Entity.WriteOnlyPropertyInfo);
            var value = adapter.GetValue();
            Assert.AreEqual(SettingsConstants.NoValue, value);
        }

        [TestMethod]
        public void ShouldCallValueChangedCallbackWhenValueChanges()
        {
            var entity = new NotifyingEntity();
            var adapter = new ClrPropertyAdapter(entity, NotifyingEntity.PropertyInfo);
            var callbackCalled = false;
            adapter.ValueChangedCallback = newValue => callbackCalled = true;
            adapter.SetValue(5);
            Assert.IsTrue(callbackCalled);
        }

        [TestMethod]
        public void ShouldGiveNewValueToValueChangedCallbackWhenValueChanges()
        {
            var entity = new NotifyingEntity();
            var adapter = new ClrPropertyAdapter(entity, NotifyingEntity.PropertyInfo);
            object value = null;
            adapter.ValueChangedCallback = newValue => value = newValue;
            entity.Property = 3;
            Assert.AreEqual(3, value);
        }

        [TestMethod]
        public void ShouldNotUseOldValueChangedCallbackIfGivenNewOne()
        {
            var entity = new NotifyingEntity();
            var adapter = new ClrPropertyAdapter(entity, NotifyingEntity.PropertyInfo);
            var callbackCallCount = 0;
            adapter.ValueChangedCallback = newValue => ++callbackCallCount;
            entity.Property = 5;
            adapter.ValueChangedCallback = newValue => { };
            entity.Property = 3;
            Assert.AreEqual(1, callbackCallCount);
        }

        [TestMethod]
        public void ShouldDoNothingWithoutValueChangedCallbackWhenValueChanges()
        {
            var entity = new NotifyingEntity();
            var adapter = new ClrPropertyAdapter(entity, NotifyingEntity.PropertyInfo);
            try
            {
                entity.Property = 5;
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ShouldDoNothingAfterValueChangedCallbackIsSetToNullWhenValueChanges()
        {
            var entity = new NotifyingEntity();
            var adapter = new ClrPropertyAdapter(entity, NotifyingEntity.PropertyInfo);
            adapter.ValueChangedCallback = newValue => { };
            adapter.ValueChangedCallback = null;
            try
            {
                entity.Property = 3;
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ShouldNotNotifyAboutValueChangesAfterBeingDisposed()
        {
            var entity = new NotifyingEntity();
            var adapter = new ClrPropertyAdapter(entity, NotifyingEntity.PropertyInfo);
            var callbackCallCount = 0;
            adapter.ValueChangedCallback = newValue => ++callbackCallCount;
            adapter.Dispose();
            entity.Property = 3;
            Assert.AreEqual(0, callbackCallCount);
        }

        private class Entity
        {
            static Entity()
            {
                var type = typeof(Entity);
                PropertyInfo = type.GetProperty("Property");
                ReadOnlyPropertyInfo = type.GetProperty("ReadOnlyProperty");
                WriteOnlyPropertyInfo = type.GetProperty("WriteOnlyProperty");
            }

            public static PropertyInfo PropertyInfo { get; private set; }

            public static PropertyInfo ReadOnlyPropertyInfo { get; private set; }

            public static PropertyInfo WriteOnlyPropertyInfo { get; private set; }

            public int Property { get; set; }

            public int ReadOnlyProperty { get { return 5; } }

            public int WriteOnlyProperty { set { } }
        }

        private sealed class NotifyingEntity : INotifyPropertyChanged
        {
            static NotifyingEntity()
            {
                PropertyInfo = typeof(NotifyingEntity).GetProperty("Property");
            }

            public static PropertyInfo PropertyInfo { get; private set; }

            public int Property
            {
                get { return _property; }
                set
                {
                    _property = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Property"));
                    }
                }
            }

            private int _property;

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
