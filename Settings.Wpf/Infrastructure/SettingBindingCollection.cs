// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TheSettings.Binding;
using TheSettings.Infrastructure;
using TheSettings.Wpf.Initialization;

namespace TheSettings.Wpf.Infrastructure
{
    /// <summary>
    /// Collection that stores setting bindings for object that owns it.
    /// Also supports setting binding providers.
    /// </summary>
    public class SettingBindingCollection : Disposable, IList
    {
        public static readonly SettingBindingCollection UnsetValue = new SettingBindingCollection(null);
        private readonly DependencyObject _owner;
        private readonly List<ISettingBindingsProvider> _providersToInitialize = new List<ISettingBindingsProvider>();
        private readonly List<ISettingBinding> _bindings = new List<ISettingBinding>();
        private Lazy<SettingsInitializer> _initializerSource;

        public SettingBindingCollection(DependencyObject owner)
        {
            _owner = owner;
            _initializerSource = new Lazy<SettingsInitializer>(
                () => new InitializerLookup().TryGetInitializer(owner));
        }

        public void AddBinding(ISettingBinding binding)
        {
            FailIfDisposed();
            _bindings.Add(binding);

            var initializer = GetInitializer();
            if (initializer != null)
            {
                initializer.QueueSettingsCollectionUpdate(this);
            }
            else
            {
                binding.UpdateTarget();
            }
        }

        public void AddBindingProvider(ISettingBindingsProvider bindingsProvider)
        {
            FailIfDisposed();
            var initializer = GetInitializer();
            if (initializer != null)
            {
                _providersToInitialize.Add(bindingsProvider);
                initializer.QueueSettingsCollectionUpdate(this);
            }
            else
            {
                var newBindings = bindingsProvider.ProvideBindings(_owner);
                AddAndUpdateBindings(newBindings);
            }
        }

        /// <summary>
        /// Used by SettingsInitializer
        /// to update bindings inside collection
        /// and run bindings providers added to it.
        /// </summary>
        internal void Initialize()
        {
            FailIfDisposed();
            RunProviders();
            UpdateBindings(_bindings);
        }

        protected override void DisposeManaged()
        {
            foreach (var binding in _bindings)
            {
                Dispose(binding);
            }
            _bindings.Clear();
        }

        private SettingsInitializer GetInitializer()
        {
            if (_initializerSource == null)
            {
                return null;
            }
            var initializer = _initializerSource.Value;
            if (initializer == null)
            {
                _initializerSource = null;
            }
            return initializer;
        }

        private void AddAndUpdateBindings(IEnumerable<ISettingBinding> bindings)
        {
            foreach (var binding in bindings)
            {
                _bindings.Add(binding);
                binding.UpdateTarget();
            }
        }

        private static void UpdateBindings(IEnumerable<ISettingBinding> bindings)
        {
            foreach (var binding in bindings)
            {
                binding.UpdateTarget();
            }
        }

        private void RunProviders()
        {
            if (_providersToInitialize.Any())
            {
                var newBindings = _providersToInitialize.SelectMany(provider => provider.ProvideBindings(_owner));
                _bindings.AddRange(newBindings);
                _providersToInitialize.Clear();
            }
        }

        // This class implements IList just so XAML reader recognizes it's a collection.
        // Furthermore, it only needs a working Add implementation, so the rest is left out.

        #region Implementation of IList

        int IList.Add(object value)
        {
            FailIfDisposed();
            var bindingProvider = value as ISettingBindingsProvider;
            if (bindingProvider != null)
            {
                AddBindingProvider(bindingProvider);
                return -1;
            }
            else
            {
                var binding = (ISettingBinding)value;
                AddBinding(binding);
                return _bindings.IndexOf(binding);
            }
        }

        void IList.Clear()
        {
            throw new System.NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new System.NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new System.NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new System.NotImplementedException();
        }

        bool IList.IsFixedSize
        {
            get { throw new System.NotImplementedException(); }
        }

        bool IList.IsReadOnly
        {
            get { throw new System.NotImplementedException(); }
        }

        void IList.Remove(object value)
        {
            throw new System.NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        object IList.this[int index]
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        void ICollection.CopyTo(System.Array array, int index)
        {
            throw new System.NotImplementedException();
        }

        int ICollection.Count
        {
            get { throw new System.NotImplementedException(); }
        }

        bool ICollection.IsSynchronized
        {
            get { throw new System.NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new System.NotImplementedException(); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion Implementation of IList
    }
}