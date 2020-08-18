using System;

namespace Toolkit.Observable
{
    public class SelectionChangingEventArgs : EventArgs
    {
        public bool CancelSelectionChange { get; set; }

        public SelectionChangingEventArgs()
        {
            CancelSelectionChange = false;
        }
    }
}