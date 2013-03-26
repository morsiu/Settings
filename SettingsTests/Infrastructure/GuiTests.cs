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
        private readonly HashSet<Window> _windowsToClose = new HashSet<Window>();

        protected void ShowThenCloseAtCleanup(Window window)
        {
            _windowsToClose.Add(window);
            window.Show();
        }

        protected T CreateShowThenCloseAtCleanup<T>()
            where T : Window, new()
        {
            var window = new T();
            ShowThenCloseAtCleanup(window);
            return window;
        }

        protected virtual void OnCleanup()
        {
            foreach (var window in _windowsToClose)
            {
                window.Close();
            }
            _windowsToClose.Clear();
        }

        [TestCleanup]
        public void Cleanup()
        {
            OnCleanup();
        }
    }
}
