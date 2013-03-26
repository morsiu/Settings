using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TheSettingsTests.Infrastructure
{
    public class GuiTests
    {
        private HashSet<Window> _windowsToClose;

        protected void ShowThenCloseAtCleanup(Window window)
        {
            _windowsToClose.Add(window);
            window.Show();
        }

        protected T CreateShowThenCloseAtCleanup<T>()
            where T : Window, new()
        {
            InitializeWindowsCleanup();
            var window = new T();
            ShowThenCloseAtCleanup(window);
            return window;
        }

        [TestInitialize]
        public void InitializeWindowsCleanup()
        {
            _windowsToClose = new HashSet<Window>();
        }

        [TestCleanup]
        public void CleanupWindows()
        {
            foreach (var window in _windowsToClose)
            {
                window.Close();
            }
            _windowsToClose = null;
        }
    }
}
