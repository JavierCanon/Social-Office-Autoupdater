﻿// TrulyObservableCollection.cs, 23.03.2020
// Copyright (C) Dominic Beger 24.03.2020

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace nUpdate.Administration.Infrastructure
{
    public sealed class TrulyObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public TrulyObservableCollection()
        {
            CollectionChanged += FullCollectionChanged;
        }

        public TrulyObservableCollection(IEnumerable<T> pItems) : this()
        {
            foreach (var item in pItems) Add(item);
        }

        private void FullCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    ((INotifyPropertyChanged) item).PropertyChanged += ItemPropertyChanged;

            if (e.OldItems == null)
                return;
            foreach (var item in e.OldItems) ((INotifyPropertyChanged) item).PropertyChanged -= ItemPropertyChanged;
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender,
                IndexOf((T) sender));
            OnCollectionChanged(args);
        }
    }
}