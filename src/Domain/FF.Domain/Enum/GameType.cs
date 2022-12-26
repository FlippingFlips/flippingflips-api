using System;

namespace FF.Domain.Enum
{
    /// <summary>
    /// Tags for the game type, emulation, display type
    /// </summary>
    [Flags]
    public enum GameType
    {
        None,
        Original    = 1 << 0,
        Recreation  = 1 << 1,
        PROC        = 1 << 2,
        FlexDmd     = 1 << 3,
        UltraDmd    = 1 << 4,
        EM          = 1 << 5,
        SS          = 1 << 6,
        PinMame     = 1 << 7,
        PinGod      = 1 << 8
    }
}
