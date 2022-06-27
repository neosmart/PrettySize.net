using System;
using System.ComponentModel;

namespace NeoSmart.PrettySize
{
    public enum UnitBase
    {
        Base2,
        Base10
    }

    public enum UnitStyle
    {
        Smart,
        Abbreviated,
        AbbreviatedLower,
        Full,
        FullLower,
    }

    #region Obsolete
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete($"Use {nameof(UnitBase)} instead")]
    public readonly struct CalculationBase
    {
        UnitBase Base2 => UnitBase.Base2;
        UnitBase Base10 => UnitBase.Base10;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete($"Use {nameof(UnitStyle)} instead")]
    public readonly struct PrintFormat
    {
        [Obsolete($"Use {nameof(UnitStyle)}.{nameof(UnitStyle.Smart)} instead")]
        readonly UnitStyle Smart => UnitStyle.Smart;
        [Obsolete($"Use {nameof(UnitStyle)}.{nameof(UnitStyle.Abbreviated)} instead")]
        readonly UnitStyle Abbreviated => UnitStyle.Abbreviated;
        [Obsolete($"Use {nameof(UnitStyle)}.{nameof(UnitStyle.AbbreviatedLower)} instead")]
        readonly UnitStyle AbbreviatedLowerCase => UnitStyle.AbbreviatedLower;
        [Obsolete($"Use {nameof(UnitStyle)}.{nameof(UnitStyle.Full)} instead")]
        readonly UnitStyle Full => UnitStyle.Full;
        [Obsolete($"Use {nameof(UnitStyle)}.{nameof(UnitStyle.FullLower)} instead")]
        readonly UnitStyle FullLowerCase => UnitStyle.FullLower;
    }
    #endregion
}
