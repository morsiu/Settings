// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheSettingsTests.Infrastructure
{
    public class GuiTests
    {
        private HashSet<Window> _windowsToClose;

        [TestCleanup]
        public void CleanupWindows()
        {
            foreach (var window in _windowsToClose)
            {
                window.Close();
            }
            _windowsToClose = null;
        }

        [TestInitialize]
        public void InitializeWindowsCleanup()
        {
            _windowsToClose = new HashSet<Window>();
        }

        protected T CreateShowThenCloseAtCleanup<T>()
            where T : Window, new()
        {
            InitializeWindowsCleanup();
            var window = new T();
            ShowThenCloseAtCleanup(window);
            return window;
        }

        protected void ShowThenCloseAtCleanup(Window window)
        {
            _windowsToClose.Add(window);
            window.Show();
            WaitForLoadedEvent(window);
        }

        private static void WaitForLoadedEvent(Window window)
        {
            var loadedEventTask = new TaskCompletionSource<object>();
            var dispatcherFrame = new DispatcherFrame();
            RoutedEventHandler onWindowLoaded =
                (s, e) =>
                {
                    dispatcherFrame.Continue = false;
                    loadedEventTask.SetResult(null);
                };
            window.Loaded += onWindowLoaded;
            Dispatcher.PushFrame(dispatcherFrame);
            loadedEventTask.Task.Wait();
            window.Loaded -= onWindowLoaded;
        }
    }
}