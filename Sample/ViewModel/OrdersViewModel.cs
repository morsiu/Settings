// Copyright (C) 2013 Łukasz Mrozek
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Sample.Model;

namespace Sample.ViewModel
{
    public class OrdersViewModel
    {
        public OrdersViewModel()
        {
            Customers = new ObservableCollection<Person>();
            foreach (var index in Enumerable.Range(1, 10))
            {
                Customers.Add(new Person(index, string.Format("Customer {0}", index)));
            }
            OrdersSource = new ObservableCollection<Order>();
            var customerIdSource = new Random();
            foreach (var index in Enumerable.Range(1, 15))
            {
                OrdersSource.Add(new Order(index, customerIdSource.Next(1, 10), string.Format("Order {0}", index)));
            }
            Orders = new ListCollectionView(OrdersSource);
        }

        public ListCollectionView Orders { get; private set; }

        public ObservableCollection<Order> OrdersSource { get; private set; }

        public ObservableCollection<Person> Customers { get; private set; }
    }
}