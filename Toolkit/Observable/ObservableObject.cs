﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Toolkit.Observable
{
    public class ObservableObject : IObservable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void SetAndNotify<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                return;
            }

            backingField = newValue;
            NotifyPropertyChanged(propertyName);
        }
    }
}