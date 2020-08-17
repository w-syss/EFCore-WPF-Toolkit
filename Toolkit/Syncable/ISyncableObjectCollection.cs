﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Toolkit.Syncable
{
    public interface ISyncableObjectCollection<T> : IEnumerable<T>, ICollection<T>
    {
        #region Collection
        public ICollectionView CollectionView { get; }
        public T SelectedItem { get; set; }

        public Predicate<T> CollectionViewFilter { get; set; }
        public string FilterText { get; set; }

        public bool IsEmpty { get; }
        #endregion
    }
}