using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettingsTests.Infrastructure;
using TheSettingsTests.MarkupTests.NamespaceExtensionTests.Windows;

namespace TheSettingsTests.MarkupTests.NamespaceExtensionTests
{
    [TestClass]
    public class ControlTemplatingNamespaceExtensionTests : GuiTests
    {
        private ControlTemplating _window;

        [TestInitialize]
        public void Initialize()
        {
            _window = CreateShowThenCloseAtCleanup<ControlTemplating>();
        }

        [TestMethod]
        public void ShouldSetNamespaceOnControlTemplateInstance()
        {
            Assert.AreEqual("\\Root\\Control\\Template", _window.ControlTemplateNamespace.Path);
        }

        [TestMethod]
        public void ShouldSetNamespaceOnControlTemplateInstanceInsideDataTemplateInstance()
        {
            Assert.AreEqual("\\Root\\Control\\Template\\Template", _window.TemplateInControlTemplateNamespace.Path);
        }

        [TestMethod]
        public void ShouldUseParentSourceNamespaceAsParentWhenSettingParentSourceNameInsideControlTemplateInstance()
        {
            Assert.AreEqual("\\Root\\Control\\Template\\ControlTemplateChild1\\ControlTemplateChild2", _window.ControlTemplateChild2Namespace.Path);
        }

        [TestMethod]
        public void ShouldNotLookForParentSourceElementOutsideOfCurrentNameScope()
        {
            Assert.AreEqual("\\ControlTemplateChild4", _window.ControlTemplateChild4Namespace.Path);
        }

        [TestMethod]
        public void ShouldNotUseAncestorNamespaceAsParentWhenNotUsingParentInsideControlTemplateInstance()
        {
            Assert.AreEqual("\\ControlTemplateChild3", _window.ControlTemplateChild3Namespace.Path);
        }
    }
}
