using System;

namespace FF.Domain.Enum
{
    [Flags]
    public enum CustomListType
    {
        None,
        Friend = 1 << 0,
        Blocked = 1 << 1,
        Hidden = 1 << 2
    }
}
