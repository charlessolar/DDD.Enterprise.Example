using System;

namespace Demo.Library.SSE
{
    [Flags]
    public enum ChangeType
    {
        // A new instance
        New = 1,
        // An instance changed
        Change = 2,
        // An instance was deleted
        Delete = 4,
        // The instance is completly different
        Refresh = 8,
        All = New | Change | Delete | Refresh
    }
}
