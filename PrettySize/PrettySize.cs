using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NeoSmart.PrettySize
{
    public readonly struct PrettySize : IFormattable, IComparable<PrettySize>, IEquatable<PrettySize>
    {
        public const long BYTE = 1;
        public const long KILOBYTE = 1000 * BYTE;
        public const long MEGABYTE = 1000 * KILOBYTE;
        public const long GIGABYTE = 1000 * MEGABYTE;
        public const long TERABYTE = 1000 * GIGABYTE;
        public const long PETABYTE = 1000 * TERABYTE;
        public const long EXABYTE  = 1000 * PETABYTE;

        public const long KIBIBYTE = 1L << 10;
        public const long MEBIBYTE = 1L << 20;
        public const long GIBIBYTE = 1L << 30;
        public const long TEBIBYTE = 1L << 40;
        public const long PEBIBYTE = 1L << 50;
        public const long EXBIBYTE = 1L << 60;

        public static PrettySize B(long value) => new PrettySize(value);
        public static PrettySize Bytes(long value) => new PrettySize(value);

        public static PrettySize Kilobytes(long value) => new PrettySize(value * BYTE);
        public static PrettySize Megabytes(long value) => new PrettySize(value * MEGABYTE);
        public static PrettySize Gigabytes(long value) => new PrettySize(value * GIGABYTE);
        public static PrettySize Terabytes(long value) => new PrettySize(value * TERABYTE);
        public static PrettySize Petabytes(long value) => new PrettySize(value * PETABYTE);
        public static PrettySize Exabytes(long value) => new PrettySize(value * EXABYTE);

        public static PrettySize KB(long value) => Kilobytes(value);
        public static PrettySize MB(long value) => Megabytes(value);
        public static PrettySize GB(long value) => Gigabytes(value);
        public static PrettySize TB(long value) => Terabytes(value);
        public static PrettySize PB(long value) => Petabytes(value);
        public static PrettySize EB(long value) => Exabytes(value);

        public static PrettySize Kibibytes(long value) => new PrettySize(value * KIBIBYTE);
        public static PrettySize Mebibytes(long value) => new PrettySize(value * MEBIBYTE);
        public static PrettySize Gibibytes(long value) => new PrettySize(value * GIBIBYTE);
        public static PrettySize Tebibytes(long value) => new PrettySize(value * TEBIBYTE);
        public static PrettySize Pebibytes(long value) => new PrettySize(value * PEBIBYTE);
        public static PrettySize Exbibytes(long value) => new PrettySize(value * EXBIBYTE);

        public static PrettySize KiB(long value) => Kibibytes(value);
        public static PrettySize MiB(long value) => Mebibytes(value);
        public static PrettySize GiB(long value) => Gibibytes(value);
        public static PrettySize TiB(long value) => Tebibytes(value);
        public static PrettySize PiB(long value) => Pebibytes(value);
        public static PrettySize EiB(long value) => Exbibytes(value);

        delegate string FormatDelegate(ulong size, UnitBase @base, UnitStyle format);

        readonly struct FormattingRule : IComparable, IComparable<FormattingRule>
        {
            public ulong LessThan { get; }
            public FormatDelegate FormatDelegate { get; }

            public int CompareTo(object other)
            {
                // We are assuming other is always IComparable to avoid overhead of "is ulong" check
                // Array.BinarySearch always used the object's ICompare even if an IComparer was specified until .NET 4.5
                // https://stackoverflow.com/a/19319601/17027
                return LessThan.CompareTo(((FormattingRule)other).LessThan);
            }

            public int CompareTo(FormattingRule other)
            {
                return LessThan.CompareTo(other.LessThan);
            }

            // .NET 2.0 doesn't have init-only setters so we need this constructor!
            public FormattingRule(ulong LessThan, FormatDelegate FormatDelegate)
            {
                this.LessThan = LessThan;
                this.FormatDelegate = FormatDelegate;
            }
        }

        readonly struct FormattingRuleComparer : IComparer<FormattingRule>
        {
            public int Compare(FormattingRule x, FormattingRule y)
            {
                return x.LessThan.CompareTo(y.LessThan);
            }
        }

        static private string PrintBytes(ulong size, UnitStyle format)
        {
            switch (format)
            {
                case UnitStyle.Abbreviated: return $"{size} B";
                case UnitStyle.AbbreviatedLower: return $"{size} b";
                case UnitStyle.Full: return size == 1 ? $"{size} Byte" : $"{size} Bytes";
                case UnitStyle.Smart:
                case UnitStyle.FullLower:
                    return size == 1 ? $"{size} byte" : $"{size} bytes";
            }

            throw new ArgumentException();
        }

        static private readonly FormattingRule[] Base10Map = new FormattingRule[]
        {
            new FormattingRule(1 * KILOBYTE, (size, @base, format) =>
            {
                return PrintBytes(size, format);
            }),
            new FormattingRule(10 * KILOBYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * KILOBYTE));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            }),
            new FormattingRule(100 * KILOBYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * KILOBYTE));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            }),
            new FormattingRule(1 * MEGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * KILOBYTE));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            }),
            new FormattingRule(10 * MEGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * MEGABYTE));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            }),
            new FormattingRule(100 * MEGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * MEGABYTE));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            }),
            new FormattingRule(1 * GIGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * MEGABYTE));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            }),
            new FormattingRule(10 * GIGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * GIGABYTE));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            }),
            new FormattingRule(100 * GIGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * GIGABYTE));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            }),
            new FormattingRule(1 * TERABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * GIGABYTE));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            }),
            new FormattingRule(10 * TERABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * TERABYTE));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            }),
            new FormattingRule(100 * TERABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * TERABYTE));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            }),
            new FormattingRule(1 * PETABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * TERABYTE));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            }),
            new FormattingRule(10 * PETABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * PETABYTE));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            }),
            new FormattingRule(100 * PETABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * PETABYTE));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            }),
            new FormattingRule(1 * EXABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * PETABYTE));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            }),
            new FormattingRule(10L * (ulong)EXABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * EXABYTE));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Exabyte", format)}";
            }),
            new FormattingRule(ulong.MaxValue, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * EXABYTE));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Exabyte", format)}";
            }),
        };

        static private readonly FormattingRule[] Base2Map = new FormattingRule[]
        {
            new FormattingRule(1 * KILOBYTE, (size, @base, format) =>
            {
                return PrintBytes(size, format);
            }),
            new FormattingRule(10 * KILOBYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * KIBIBYTE));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Kibibyte", format)}";
            }),
            new FormattingRule(100 * KILOBYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * KIBIBYTE));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Kibibyte", format)}";
            }),
            new FormattingRule(1 * MEGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * KIBIBYTE));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Kibibyte", format)}";
            }),
            new FormattingRule(10 * MEGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * MEBIBYTE));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            }),
            new FormattingRule(100 * MEGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * MEBIBYTE));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            }),
            new FormattingRule(1 * GIGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * MEBIBYTE));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            }),
            new FormattingRule(10 * GIGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * GIBIBYTE));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            }),
            new FormattingRule(100 * GIGABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * GIBIBYTE));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            }),
            new FormattingRule(1 * TERABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * GIBIBYTE));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            }),
            new FormattingRule(10 * TERABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * TEBIBYTE));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            }),
            new FormattingRule(100 * TERABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * TEBIBYTE));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            }),
            new FormattingRule(1 * PETABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * TEBIBYTE));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            }),
            new FormattingRule(10 * PETABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * PEBIBYTE));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Pebibyte", format)}";
            }),
            new FormattingRule(100 * PETABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * PEBIBYTE));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Pebibyte", format)}";
            }),
            new FormattingRule(1 * EXABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * PEBIBYTE));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Pebibyte", format)}";
            }),
            new FormattingRule(10 * (ulong)EXABYTE, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * EXBIBYTE));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Exbibyte", format)}";
            }),
            new FormattingRule(ulong.MaxValue, (size, @base, format) =>
            {
                var formattedSize = (size / (1D * EXBIBYTE));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Exbibyte", format)}";
            }),
        };

        public long TotalBytes { get; }

        public PrettySize(long bytes)
        {
            TotalBytes = bytes;
        }

        public PrettySize(ulong bytes)
        {
            TotalBytes = (long)bytes;
        }

        public override string ToString()
        {
            return Format(TotalBytes, UnitBase.Base2, UnitStyle.Smart);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // throw new NotImplementedException();
            return ToString();
        }

        public string Format(UnitBase @base = UnitBase.Base2, UnitStyle format = UnitStyle.Smart)
        {
            return Format(TotalBytes, @base, format);
        }

        public static string Format(long size, UnitBase @base = UnitBase.Base2, UnitStyle format = UnitStyle.Smart)
        {
            bool neg = size < 0;
            ulong fmt_size = size switch
            {
                // Math.Abs(long.MinValue) is out of range for long so we handle it separately
                long.MinValue => ((ulong)long.MaxValue) + 1,
                < 0 => (ulong) Math.Abs(size),
                _ => (ulong)size,
            };

            var formatted = Format(fmt_size, @base, format);
            return neg ? "-" + formatted : formatted;
        }

        public static string Format(ulong size, UnitBase @base = UnitBase.Base2, UnitStyle format = UnitStyle.Smart)
        {
            if (@base == UnitBase.Base2)
            {
                var searchIndex = Array.BinarySearch(Base2Map, new FormattingRule(size, null), new FormattingRuleComparer());
                if (searchIndex < 0)
                {
                    searchIndex = ~searchIndex;
                    return Base2Map[searchIndex].FormatDelegate(size, @base, format);
                }
                return Base2Map[searchIndex + 1].FormatDelegate(size, @base, format);
            }
            else
            {
                var searchIndex = Array.BinarySearch(Base10Map, new FormattingRule(size, null), new FormattingRuleComparer());
                if (searchIndex < 0)
                {
                    searchIndex = ~searchIndex;
                    return Base10Map[searchIndex].FormatDelegate(size, @base, format);
                }
                return Base10Map[searchIndex+1].FormatDelegate(size, @base, format);
            }
        }

        private static string FormatUnitBase2(double formattedSize, string unit, UnitStyle format)
        {
            if (formattedSize != 1.0D)
            {
                unit += "s";
            }

            if (format == UnitStyle.Smart || format == UnitStyle.Abbreviated)
            {
                return unit[0] + "iB";
            }
            else if (format == UnitStyle.Full)
            {
                return unit;
            }
            else if (format == UnitStyle.FullLower)
            {
                return unit.ToLowerInvariant();
            }
            else if (format == UnitStyle.AbbreviatedLower)
            {
                return char.ToLowerInvariant(unit[0]) + "ib";
            }

            throw new ArgumentException();
        }

        private static string FormatUnitBase10(double formattedSize, string unit, UnitStyle format)
        {
            if (formattedSize != 1.0D)
            {
                unit += "s";
            }

            if (format == UnitStyle.Smart || format == UnitStyle.Abbreviated)
            {
                return unit[0] + "B";
            }
            else if (format == UnitStyle.Full)
            {
                return unit;
            }
            else if (format == UnitStyle.FullLower)
            {
                return unit.ToLowerInvariant();
            }
            else if (format == UnitStyle.AbbreviatedLower)
            {
                return char.ToLowerInvariant(unit[0]) + "b";
            }

            throw new ArgumentException();
        }

        public static PrettySize operator +(PrettySize lhs, PrettySize rhs)
        {
            return new PrettySize(lhs.TotalBytes + rhs.TotalBytes);
        }

        public static PrettySize operator -(PrettySize lhs, PrettySize rhs)
        {
            return new PrettySize(lhs.TotalBytes - rhs.TotalBytes);
        }

        public static PrettySize operator *(PrettySize lhs, long rhs)
        {
            return new PrettySize(lhs.TotalBytes * rhs);
        }

        public static PrettySize operator *(long lhs, PrettySize rhs)
        {
            return new PrettySize(rhs.TotalBytes * lhs);
        }

        public static PrettySize operator /(PrettySize lhs, long rhs)
        {
            return new PrettySize((long) (((double)lhs.TotalBytes) / rhs));
        }

        public int CompareTo(PrettySize other)
        {
            return TotalBytes.CompareTo(other.TotalBytes);
        }

        public bool Equals(PrettySize other)
        {
            return TotalBytes.Equals(other.TotalBytes);
        }

        public override bool Equals(object obj)
        {
            return obj is PrettySize other && Equals(other);
        }

        public override int GetHashCode()
        {
#if NETSTANDARD2_1_OR_GREATER
            return HashCode.Combine(TotalBytes);
#else
            return TotalBytes.GetHashCode();
#endif
        }

        public static bool operator ==(PrettySize lhs, PrettySize rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(PrettySize lhs, PrettySize rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator <(PrettySize lhs, PrettySize rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator >(PrettySize lhs, PrettySize rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator <=(PrettySize lhs, PrettySize rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public static bool operator >=(PrettySize lhs, PrettySize rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        #region Obsolete
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete($"Use {nameof(KIBIBYTE)} instead")]
        public const long KEBIBYTE = KIBIBYTE;
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete($"Use {nameof(Kibibytes)} instead")]
        public static PrettySize Kebibytes(long value) => Kibibytes(value);
        #endregion
    }
}
