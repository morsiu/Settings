// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using System;

namespace TheSettingsTests
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
