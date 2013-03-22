using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SettingsTests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void ShouldGetDistinctEmptyCollectionsAsSettingsPropertyDefaultValueForDifferentObjects()
        {
            var objectA = new DependencyObject();
            var objectB = new DependencyObject();
            var collectionA = Settings.Binding.Wpf.Markup.Settings.GetSettings(objectA);
            var collectionB = Settings.Binding.Wpf.Markup.Settings.GetSettings(objectB);
            Assert.AreNotSame(collectionA, collectionB);
        }
    }
}
