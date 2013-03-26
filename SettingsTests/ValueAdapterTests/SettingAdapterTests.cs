using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettings;
using TheSettings.Binding.ValueAdapters;
using TheSettings.Binding.Wpf;

namespace TheSettingsTests.ValueAdapterTests
{
    [TestClass]
    public class SettingAdapterTests
    {
        Mock<ISettingsStoreAccessor> _accessor;

        [TestInitialize]
        public void Initialize()
        {
            _accessor = new Mock<ISettingsStoreAccessor>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullSettingsStoreAccessor()
        {
            new SettingAdapter(null, null, new SettingsNamespace("Namespace"), "Setting");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullNamespace()
        {
            new SettingAdapter(_accessor.Object, null, null, "Setting");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullSettingKey()
        {
            new SettingAdapter(_accessor.Object, null, new SettingsNamespace("Namespace"), null);
        }

        [TestMethod]
        public void ShouldGetValueFromSettingsStoreAccessor()
        {
            _accessor.Setup<object>(o => 
                o.GetSetting(It.IsAny<object>(), It.IsAny<SettingsNamespace>(), It.IsAny<string>()));
            var _adapter = new SettingAdapter(_accessor.Object, null, new SettingsNamespace("Namespace"), "Setting");
            var value = _adapter.GetValue();
            _accessor.VerifyAll();
        }

        [TestMethod]
        public void ShouldPassStoreKeyWhenGettingValue()
        {
            var storeKey = "Key";
            _accessor.Setup<object>(o => 
                o.GetSetting(storeKey, It.IsAny<SettingsNamespace>(), It.IsAny<string>()));
            var _adapter = new SettingAdapter(_accessor.Object, storeKey, new SettingsNamespace("Namespace"), "Setting");
            var value = _adapter.GetValue();
            _accessor.VerifyAll();
        }

        [TestMethod]
        public void ShouldPassNamespaceKeyWhenGettingValue()
        {
            var @namespace = new SettingsNamespace("Namespace");
            _accessor.Setup<object>(o => 
                o.GetSetting(It.IsAny<object>(), @namespace, It.IsAny<string>()));
            var _adapter = new SettingAdapter(_accessor.Object, null, @namespace, "Setting");
            var value = _adapter.GetValue();
            _accessor.VerifyAll();
        }

        [TestMethod]
        public void ShouldPassSettingNameKeyWhenGettingValue()
        {
            var settingName = "Setting";
            _accessor.Setup<object>(o => 
                o.GetSetting(It.IsAny<object>(), It.IsAny<SettingsNamespace>(), settingName));
            var _adapter = new SettingAdapter(_accessor.Object, null, new SettingsNamespace("Namespace"), settingName);
            var value = _adapter.GetValue();
            _accessor.VerifyAll();
        }

        [TestMethod]
        public void ShouldSetValueUsingSettingsStoreAccessor()
        {
            _accessor.Setup(o => 
                o.SetSetting(It.IsAny<object>(), It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.IsAny<object>()));
            var _adapter = new SettingAdapter(_accessor.Object, null, new SettingsNamespace("Namespace"), "Setting");
            _adapter.SetValue(null);
            _accessor.VerifyAll();
        }


        [TestMethod]
        public void ShouldPassStoreKeyWhenSettingValue()
        {
            var storeKey = "Key";
            _accessor.Setup(o => 
                o.SetSetting(storeKey, It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.IsAny<object>()));
            var _adapter = new SettingAdapter(_accessor.Object, storeKey, new SettingsNamespace("Namespace"), "Setting");
            _adapter.SetValue(null);
            _accessor.VerifyAll();
        }

        [TestMethod]
        public void ShouldPassNamespaceKeyWhenSettingValue()
        {
            var @namespace = new SettingsNamespace("Namespace");
            _accessor.Setup(o => 
                o.SetSetting(It.IsAny<object>(), It.IsAny<SettingsNamespace>(), It.IsAny<string>(), It.IsAny<object>()));
            var _adapter = new SettingAdapter(_accessor.Object, null, @namespace, "Setting");
            _adapter.SetValue(null);
            _accessor.VerifyAll();
        }

        [TestMethod]
        public void ShouldPassNewValueWhenSettingValue()
        {
            var value = new object();
            _accessor.Setup(o => 
                o.SetSetting(It.IsAny<object>(), It.IsAny<SettingsNamespace>(), It.IsAny<string>(), value));
            var _adapter = new SettingAdapter(_accessor.Object, null, new SettingsNamespace("Namespace"), "Setting");
            _adapter.SetValue(value);
            _accessor.VerifyAll();
        }

        [TestMethod]
        public void ShouldPassSettingNameKeyWhenSettingValue()
        {
            var settingName = "Setting";
            _accessor.Setup(o => 
                o.SetSetting(It.IsAny<object>(), It.IsAny<SettingsNamespace>(), settingName, It.IsAny<object>()));
            var _adapter = new SettingAdapter(_accessor.Object, null, new SettingsNamespace("Namespace"), settingName);
            _adapter.SetValue(null);
            _accessor.VerifyAll();
        }
    }
}
