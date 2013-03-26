using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSettingsTests.Infrastructure;

namespace TheSettingsTests.MarkupTests.NamespaceExtension
{
    [TestClass]
    public class BasicNamespaceExtensionTests : GuiTests
    {
        [TestMethod]
        public void ShouldSetNamespace()
        {
            var window = CreateShowThenCloseAtCleanup<NamespaceInheritance>();
            Assert.AreEqual("\\Namespace", window.RootNamespace.Path);
        }

        [TestMethod]
        public void ShouldInheritNamespaceAlongVisualwindow()
        {
            var window = CreateShowThenCloseAtCleanup<NamespaceInheritance>();
            Assert.AreEqual("\\Namespace", window.Level1Namespace.Path);
        }

        [TestMethod]
        public void ShouldInheritAncestorsPathAsParentPath()
        {
            var window = CreateShowThenCloseAtCleanup<NamespaceParent>();
            Assert.AreEqual("\\Root\\Child1", window.Child1Namespace.Path);
        }

        [TestMethod]
        public void ShouldNotInheritAncestorsPathAsParentIfNotUsingParent()
        {
            var window = CreateShowThenCloseAtCleanup<NamespaceParent>();
            Assert.AreEqual("\\Child2", window.Child2Namespace.Path);
        }

        [TestMethod]
        public void ShouldUseSourcePathAsParentIfUsingParentSourceName()
        {
            var window = CreateShowThenCloseAtCleanup<NamespaceParent>();
            Assert.AreEqual("\\Root\\Child3\\Child4", window.Child4Namespace.Path);
        }
    }
}
