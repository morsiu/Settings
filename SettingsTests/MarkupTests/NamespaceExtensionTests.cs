using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheSettingsTests.MarkupTests
{
    [TestClass]
    public class NamespaceExtensionTests
    {
        [TestMethod]
        public void ShouldSetNamespace()
        {
            var tree = new NamespaceInheritance();
            Assert.AreEqual("\\Namespace", tree.RootNamespace.Path);
        }

        [TestMethod]
        public void ShouldInheritNamespaceAlongVisualTree()
        {
            var tree = new NamespaceInheritance();
            Assert.AreEqual("\\Namespace", tree.Level1Namespace.Path);
        }

        [TestMethod]
        public void ShouldInheritNamespaceToNestedControl()
        {
            var tree = new NamespaceInheritance();
            Assert.AreEqual("\\Namespace", tree.NestedLevel1Namespace.Path);
        }

        [TestMethod]
        public void ShouldInheritAncestorsPathAsParentPath()
        {
            var tree = new NamespaceParent();
            Assert.AreEqual("\\Root\\Child1", tree.Child1Namespace.Path);
        }

        [TestMethod]
        public void ShouldNotInheritAncestorsPathAsParentIfNotUsingParent()
        {
            var tree = new NamespaceParent();
            Assert.AreEqual("\\Child2", tree.Child2Namespace.Path);
        }

        [TestMethod]
        public void ShouldUseSourcePathAsParentIfUsingParentSourceName()
        {
            var tree = new NamespaceParent();
            Assert.AreEqual("\\Root\\Child3\\Child4", tree.Child4Namespace.Path);
        }
    }
}
