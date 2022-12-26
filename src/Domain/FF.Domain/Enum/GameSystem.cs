using System;

namespace FF.Domain.Enum
{
    /// <summary>
    /// Simulator type
    /// </summary>
    [Flags]
    public enum GameSystem
    {
        None,
        VisualPinball = 1 << 0,
        FuturePinball = 1 << 1,
        RealPinball   = 1 << 2,
    }
}