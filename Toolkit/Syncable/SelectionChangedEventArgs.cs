using System;

namespace Toolkit.Syncable
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public bool CancelSelectionChange { get; set; }

        public SelectionChangedEventArgs()
        {
            CancelSelectionChange = false;
        }
    }
}