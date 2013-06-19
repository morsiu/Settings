// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TheSettings.Binding;
using TheSettings.Infrastructure;

namespace TheSettings.Wpf.Binding.Adapters
{
    public class SelectorSortDescriptionsAdapter : Disposable, ICollectionAdapter
    {
        private readonly IValueAdapter _collectionViewAdapter;
        private ICollectionView _collectionView;
        private CollectionChangedCallbackHandler _collectionChangedCallback;
        private bool _isUpdatingSortDescriptions;

        public SelectorSortDescriptionsAdapter(IValueAdapter collectionViewAdapter)
        {
            if (collectionViewAdapter == null) throw new ArgumentNullException("collectionViewAdapter");
            _collectionChangedCallback = _ => { };
            _collectionViewAdapter = collectionViewAdapter;
            _collectionViewAdapter.ValueChangedCallback = OnCollectionViewChanged;
            RefreshCollectionView();
        }

        private void RefreshCollectionView()
        {
            UnbindFromCollectionView();

            _collectionView = (ICollectionView)_collectionViewAdapter.GetValue();
            ((INotifyCollectionChanged)_collectionView.SortDescriptions).CollectionChanged += OnSortDescriptionsChanged;
        }

        private void UnbindFromCollectionView()
        {
            if (_collectionView != null)
            {
                ((INotifyCollectionChanged)_collectionView.SortDescriptions).CollectionChanged -= OnSortDescriptionsChanged;
            }
        }

        private void OnSortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_isUpdatingSortDescriptions)
            {
                return;
            }
            _collectionChangedCallback(e);
        }

        private void OnCollectionViewChanged(object newValue)
        {
            RefreshCollectionView();
        }

        public CollectionChangedCallbackHandler CollectionChangedCallback
        {
            set { _collectionChangedCallback = value ?? (_ => { }); }
        }

        public IEnumerable GetItems()
        {
            return _collectionView.SortDescriptions;
        }

        public void SetItems(IEnumerable items)
        {
            _isUpdatingSortDescriptions = true;
            try
            {
                _collectionView.SortDescriptions.Clear();
                foreach (var sortDescription in items.OfType<SortDescription>())
                {
                    _collectionView.SortDescriptions.Add(sortDescription);
                }
            }
            finally
            {
                _isUpdatingSortDescriptions = false;
            }
        }

        protected override void DisposeManaged()
        {
            UnbindFromCollectionView();
            Dispose(_collectionViewAdapter);
        }
    }
}