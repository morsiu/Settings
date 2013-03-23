using Microsoft.VisualStudio.TestTools.UnitTesting;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsTests
{
    [TestClass]
    public class SettingsNamespaceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullName()
        {
            new SettingsNamespace(SettingsNamespace.None, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullParent()
        {
            new SettingsNamespace(null, "A name");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShouldThrowWhenConstructedWithNullNameAndDefaultParent()
        {
            new SettingsNamespace(null);
        }

        [TestMethod]
        public void ShouldUseGivenName()
        {
            const string name = "Name";
            var ns = new SettingsNamespace(SettingsNamespace.None, name);
            Assert.AreEqual(name, ns.Name);
        }

        [TestMethod]
        public void ShouldUseGivenParent()
        {
            var parent = new SettingsNamespace("Parent");
            var ns = new SettingsNamespace(parent, "Name");
            Assert.AreSame(parent, ns.Parent);
        }

        [TestMethod]
        public void ShouldUseGivenNameWithDefaultParent()
        {
            const string name = "Name";
            var ns = new SettingsNamespace(name);
            Assert.AreEqual(name, ns.Name);
        }

        [TestMethod]
        public void ShouldUseNoneNamespaceAsDefaultParent()
        {
            var ns = new SettingsNamespace("Name");
            Assert.AreSame(SettingsNamespace.None, ns.Parent);
        }

        [TestMethod]
        public void ShouldGiveEmptyPathForNoneNamespace()
        {
            var ns = SettingsNamespace.None;
            Assert.AreEqual("", ns.Path);
        }

        [TestMethod]
        public void ShouldIncludeParentInPath()
        {
            var parent = new SettingsNamespace("Parent");
            var ns = new SettingsNamespace(parent, "Child");
            Assert.AreEqual("\\Parent\\Child", ns.Path);
        }

        [TestMethod]
        public void ShouldIncludeEmptyParentForSettingWithNoParent()
        {
            var ns = new SettingsNamespace("Child");
            Assert.AreEqual("\\Child", ns.Path);
        }
    }
}
