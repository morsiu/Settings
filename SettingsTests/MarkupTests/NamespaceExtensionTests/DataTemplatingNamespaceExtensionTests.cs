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

namespace TheSettingsTests.MarkupTests.NamespaceExtensionTests
{
    [TestClass]
    public class DataTemplatingNamespaceExtensionTests
    {
        [TestMethod]
        [Ignore]
        public void ShouldSetNamespaceOnDataTemplateInstance()
        {
            var tree = new DataTemplating();
            Assert.AreEqual("\\Root", tree.TemplateNamespace.Path);
        }
    }
}
