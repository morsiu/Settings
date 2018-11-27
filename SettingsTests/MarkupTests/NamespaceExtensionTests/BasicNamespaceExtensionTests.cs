// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void ShouldInheritNamespaceAlongVisualTree()
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
