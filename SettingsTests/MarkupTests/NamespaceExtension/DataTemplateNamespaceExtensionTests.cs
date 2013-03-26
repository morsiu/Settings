using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace TheSettingsTests.MarkupTests
{
    [TestClass]
    public class DataTemplateNamespaceExtensionTests
    {
        [TestMethod]
        [Ignore]
        public void ShouldSetNamespaceOnDataTemplateInstance()
        {
            var tree = new DataTemplates();
            if (!tree.IsInitialized)
            {
                var waiter = new ManualResetEvent(false);
                tree.Initialized += (s, e) => waiter.Set();
                waiter.WaitOne();
            }
            Assert.AreEqual("\\Root", tree.TemplateNamespace.Path);
        }
    }
}
