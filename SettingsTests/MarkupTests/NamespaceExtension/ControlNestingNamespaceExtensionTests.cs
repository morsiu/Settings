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
    public class ControlNestingNamespaceExtensionTests : GuiTests
    {
        [TestMethod]
        public void ShouldInheritNamespaceToNestedControl()
        {
            var window = CreateShowThenCloseAtCleanup<NamespaceInheritance>();
            Assert.AreEqual("\\Namespace", window.NestedLevel1Namespace.Path);
        }

        [TestMethod]
        public void ShouldSetNamespaceInNestedControlWithOuterControlsNamespaceAsParent()
        {
            var window = CreateShowThenCloseAtCleanup<NamespaceInheritance>();
            Assert.AreEqual("\\Namespace\\Nested", window.NestedLevel2Namespace.Path);
        }
    }
}
