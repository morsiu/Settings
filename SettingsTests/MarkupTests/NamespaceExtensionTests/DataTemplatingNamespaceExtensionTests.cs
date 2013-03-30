using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using TheSettingsTests.MarkupTests.NamespaceExtensionTests.Windows;
using TheSettingsTests.Infrastructure;

namespace TheSettingsTests.MarkupTests.NamespaceExtensionTests
{
    [TestClass]
    public class DataTemplatingNamespaceExtensionTests : GuiTests
    {
        private DataTemplating _window;

        [TestInitialize]
        public void Initialize()
        {
            _window = CreateShowThenCloseAtCleanup<DataTemplating>();
        }

        [TestMethod]
        public void ShouldSetNamespaceOnDataTemplateInstance()
        {
            Assert.AreEqual("\\Root\\Template", _window.TemplateNamespace.Path);
        }

        [TestMethod]
        public void ShouldSetNamespaceOnDataTemplateInstanceInsideDataTemplateInstance()
        {
            Assert.AreEqual("\\Root\\Template\\Template", _window.TemplateInTemplateNamespace.Path);
        }

        [TestMethod]
        public void ShouldUseParentSourceNamespaceAsParentWhenSettingParentSourceNameInsideDataTemplateInstance()
        {
            Assert.AreEqual("\\Root\\Template\\TemplateChild1\\TemplateChild2", _window.TemplateChild2Namespace.Path);
        }

        [TestMethod]
        public void ShouldNotUseAncestorNamespaceAsParentWhenNotUsingParentInsideDataTemplateInstance()
        {
            Assert.AreEqual("\\TemplateChild3", _window.TemplateChild3Namespace.Path);
        }
    }
}
