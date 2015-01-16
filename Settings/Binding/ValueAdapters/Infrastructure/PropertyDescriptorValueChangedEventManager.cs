// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace TheSettings.Binding.ValueAdapters.Infrastructure
{
    internal sealed class PropertyDescriptorValueChangedEventManager
    {
        private static readonly Lazy<PropertyDescriptorValueChangedEventManager> _globalManager
            = new Lazy<PropertyDescriptorValueChangedEventManager>(CreateGlobalManager, LazyThreadSafetyMode.PublicationOnly);

        private readonly ListenerCollection _listeners = new ListenerCollection();

        public static void AddHandler(object source, EventHandler handler, PropertyDescriptor property)
        {
            _globalManager.Value.AddHandler(source, property, handler);
        }

        public static void RemoveHandler(object source, EventHandler handler, PropertyDescriptor property)
        {
            _globalManager.Value.RemoveHandler(source, property, handler);
        }

        public void AddHandler(object source, PropertyDescriptor property, EventHandler handler)
        {
            var listener = new Listener(source, property, handler);
            _listeners.AddListener(listener);
            listener.Enable();
        }

        public void RemoveHandler(object source, PropertyDescriptor property, EventHandler handler)
        {
            var listener = new Listener(source, property, handler);
            _listeners.RemoveListener(listener);
            listener.Disable();
        }

        private static PropertyDescriptorValueChangedEventManager CreateGlobalManager()
        {
            var manager = new PropertyDescriptorValueChangedEventManager();
            new PeriodicTask(manager.Cleanup, TimeSpan.FromMinutes(5)).Start();
            return manager;
        }

        public void Cleanup()
        {
            _listeners.CleanupListeners();
        }

        private sealed class Listener
        {
            private readonly PropertyDescriptor _descriptor;
            private readonly EventHandler _handler;
            private readonly WeakReference<object> _source;

            public Listener(object source, PropertyDescriptor descriptor, EventHandler handler)
            {
                _source = new WeakReference<object>(source);
                _descriptor = descriptor;
                _handler = handler;
            }

            public bool IsSourceDead
            {
                get
                {
                    object source;
                    return !_source.TryGetTarget(out source);
                }
            }

            public void Disable()
            {
                object source;
                if (_source.TryGetTarget(out source))
                {
                    _descriptor.RemoveValueChanged(source, _handler);
                }
            }

            public void Enable()
            {
                object source;
                if (_source.TryGetTarget(out source))
                {
                    _descriptor.AddValueChanged(source, _handler);
                }
            }
        }

        private sealed class ListenerCollection
        {
            private readonly List<Listener> _listeners = new List<Listener>();
            private readonly object _listenersLock = new object();

            public void AddListener(Listener listener)
            {
                lock (_listenersLock)
                {
                    _listeners.Add(listener);
                }
            }

            public void CleanupListeners()
            {
                lock (_listenersLock)
                {
                    _listeners.RemoveAll(l => l.IsSourceDead);
                }
            }

            public void RemoveListener(Listener listener)
            {
                lock (_listenersLock)
                {
                    _listeners.Remove(listener);
                }
            }
        }

        private sealed class PeriodicTask
        {
            private readonly Action _action;
            private readonly TimeSpan _period;
            private readonly Lazy<Task> _task;

            public PeriodicTask(Action action, TimeSpan period)
            {
                _action = action;
                _period = period;
                _task = new Lazy<Task>(LoopAction, LazyThreadSafetyMode.ExecutionAndPublication);
            }

            public void Start()
            {
                var task = _task.Value;
            }

            private async Task LoopAction()
            {
                while (true)
                {
                    await Task.Delay(_period).ConfigureAwait(false);
                    _action();
                }
            }
        }
    }
}