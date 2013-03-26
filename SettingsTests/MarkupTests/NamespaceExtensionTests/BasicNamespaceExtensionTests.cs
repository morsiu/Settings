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
    public class BasicNamespaceExtensionTests : GuiTests
    {
        private Basic _window;

        [TestInitialize]
        public void Initialize()
        {
            _window = CreateShowThenCloseAtCleanup<Basic>();
        }

        [TestMethod]
        public void ShouldSetNamespace()
        {
            Assert.AreEqual("\\Root", _window.RootNamespace.Path);
        }

        [TestMethod]
        public void ShouldInheritNamespaceAlongVisualwindow()
        {
            Assert.AreEqual("\\Root", _window.Level1Namespace.Path);
        }

        [TestMethod]
        public void ShouldInheritAncestorsPathAsParentPath()
        {
            Assert.AreEqual("\\Root\\Child1", _window.Child1Namespace.Path);
        }

        [TestMethod]
        public void ShouldNotInheritAncestorsPathAsParentIfNotUsingParent()
        {
            Assert.AreEqual("\\Child2", _window.Child2Namespace.Path);
        }

        [TestMethod]
        public void ShouldUseSpecifiedControlsNamespaceAsParentIfUsingParentSourceName()
        {
            Assert.AreEqual("\\Root\\Child3\\Child4", _window.Child4Namespace.Path);
        }
    }
}
