using Microsoft.VisualStudio.TestTools.UnitTesting;
using Settings;
using Settings.Binding.ValueAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SettingsTests.ValueAdapterTests
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
    }
}
