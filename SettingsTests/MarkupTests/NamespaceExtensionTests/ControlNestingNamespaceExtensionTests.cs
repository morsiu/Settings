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
    public class ControlNestingNamespaceExtensionTests : GuiTests
    {
        private ControlNesting _window;

        [TestInitialize]
        public void Initialize()
        {
            _window = CreateShowThenCloseAtCleanup<ControlNesting>();
        }

        [TestMethod]
        public void ShouldInheritNamespaceToNestedControl()
        {
            Assert.AreEqual("\\Root", _window.NestedLevel1Namespace.Path);
        }

        [TestMethod]
        public void ShouldSetNamespaceInNestedControlWithParentInheritedFromContainingControl()
        {
            Assert.AreEqual("\\Root\\Nested", _window.NestedLevel2Namespace.Path);
        }
    }
}
