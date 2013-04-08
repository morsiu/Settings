// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
        public void ShouldNotLookForParentSourceElementOutsideOfCurrentNameScope()
        {
            Assert.AreEqual("\\TemplateChild4", _window.TemplateChild4Namespace.Path);
        }

        [TestMethod]
        public void ShouldNotLookForParentSourceElementInsideInnerNameScope()
        {
            Assert.AreEqual("\\TemplateChild5", _window.TemplateChild5Namespace.Path);
        }
    }
}
