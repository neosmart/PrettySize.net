using System;
using System.Collections.Generic;

namespace NeoSmart.PrettySize
{
    public readonly struct PrettySize : IFormattable
    {
        public const long Byte = 1;
        public const long Kilobyte = 1000 * Byte;
        public const long Megabyte = 1000 * Kilobyte;
        public const long Gigabyte = 1000 * Megabyte;
        public const long Terabyte = 1000 * Gigabyte;
        public const long Petabyte = 1000 * Terabyte;
        public const long Exabyte  = 1000 * Petabyte;

        public const long Kibibyte = 1L << 10;
        public const long Mebibyte = 1L << 20;
        public const long Gibibyte = 1L << 30;
        public const long Tebibyte = 1L << 40;
        public const long Pebibyte = 1L << 50;
        public const long Exbibyte = 1L << 60;

        public const long B = Byte;
        public const long KB = Kilobyte;
        public const long KiB = Kibibyte;
        public const long MB = Megabyte;
        public const long MiB = Mebibyte;
        public const long GB = Gigabyte;
        public const long GiB = Gibibyte;
        public const long TB = Terabyte;
        public const long TiB = Tebibyte;

        delegate string FormatDelegate(ulong size, CalculationBase @base, PrintFormat format);

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

        static private string PrintBytes(ulong size, PrintFormat format)
        {
            switch (format)
            {
                case PrintFormat.Abbreviated: return $"{size} B";
                case PrintFormat.AbbreviatedLowerCase: return $"{size} b";
                case PrintFormat.Full: return size == 1 ? $"{size} Byte" : $"{size} Bytes";
                case PrintFormat.Smart:
                case PrintFormat.FullLowerCase:
                    return size == 1 ? $"{size} byte" : $"{size} bytes";
            }

            throw new ArgumentException();
        }

        static private readonly FormattingRule[] Base10Map = new FormattingRule[]
        {
            new FormattingRule(0, null), // this should never be reached
            new FormattingRule(1 * Kilobyte, (size, @base, format) =>
            {
                return PrintBytes(size, format);
            }),
            new FormattingRule(10 * Kilobyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kilobyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            }),
            new FormattingRule(100 * Kilobyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kilobyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            }),
            new FormattingRule(1 * Megabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kilobyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            }),
            new FormattingRule(10 * Megabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Megabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            }),
            new FormattingRule(100 * Megabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Megabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            }),
            new FormattingRule(1 * Gigabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Megabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            }),
            new FormattingRule(10 * Gigabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gigabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            }),
            new FormattingRule(100 * Gigabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gigabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            }),
            new FormattingRule(1 * Terabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gigabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            }),
            new FormattingRule(10 * Terabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Terabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            }),
            new FormattingRule(100 * Terabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Terabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            }),
            new FormattingRule(1 * Petabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Terabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            }),
            new FormattingRule(10 * Petabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Petabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            }),
            new FormattingRule(100 * Petabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Petabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            }),
            new FormattingRule(1 * Exabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Petabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            }),
            new FormattingRule(10L * (ulong)Exabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Exabyte", format)}";
            }),
            new FormattingRule(ulong.MaxValue, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Exabyte", format)}";
            }),
        };

        static private readonly FormattingRule[] Base2Map = new FormattingRule[]
        {
            new FormattingRule (0, null), // this should never be reached
            new FormattingRule(1 * Kilobyte, (size, @base, format) =>
            {
                return PrintBytes(size, format);
            }),
            new FormattingRule(10 * Kilobyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kebibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Kebibyte", format)}";
            }),
            new FormattingRule(100 * Kilobyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kebibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Kebibyte", format)}";
            }),
            new FormattingRule(1 * Megabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kebibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Kebibyte", format)}";
            }),
            new FormattingRule(10 * Megabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Mebibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            }),
            new FormattingRule(100 * Megabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Mebibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            }),
            new FormattingRule(1 * Gigabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Mebibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            }),
            new FormattingRule(10 * Gigabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gibibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            }),
            new FormattingRule(100 * Gigabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gibibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            }),
            new FormattingRule(1 * Terabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gibibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            }),
            new FormattingRule(10 * Terabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Tebibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            }),
            new FormattingRule(100 * Terabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Tebibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            }),
            new FormattingRule(1 * Petabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Tebibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            }),
            new FormattingRule(10 * Petabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Pebibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Pebibyte", format)}";
            }),
            new FormattingRule(100 * Petabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Pebibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Pebibyte", format)}";
            }),
            new FormattingRule(1 * Exabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Pebibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Pebibyte", format)}";
            }),
            new FormattingRule(10 * (ulong)Exabyte, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exbibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Exbibyte", format)}";
            }),
            new FormattingRule(ulong.MaxValue, (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exbibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Exbibyte", format)}";
            }),
        };

        public long Bytes { get; }

        public PrettySize(long bytes)
        {
            Bytes = bytes;
        }

        public PrettySize(ulong bytes)
        {
            Bytes = (long)bytes;
        }

        public override string ToString()
        {
            return Format(Bytes, CalculationBase.Base2, PrintFormat.Smart);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // throw new NotImplementedException();
            return ToString();
        }

        public static string Format(long size, CalculationBase @base = CalculationBase.Base2, PrintFormat format = PrintFormat.Smart)
        {
            return Format((ulong)size, @base, format);
        }

        public static string Format(ulong size, CalculationBase @base = CalculationBase.Base2, PrintFormat format = PrintFormat.Smart)
        {
            if (@base == CalculationBase.Base2)
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

        private static string FormatUnitBase2(decimal formattedSize, string unit, PrintFormat format)
        {
            if (formattedSize != 1.0M)
            {
                unit += "s";
            }

            if (format == PrintFormat.Smart || format == PrintFormat.Abbreviated)
            {
                return unit[0] + "iB";
            }
            else if (format == PrintFormat.Full)
            {
                return unit;
            }
            else if (format == PrintFormat.FullLowerCase)
            {
                return unit.ToLowerInvariant();
            }
            else if (format == PrintFormat.AbbreviatedLowerCase)
            {
                return char.ToLowerInvariant(unit[0]) + "ib";
            }

            throw new ArgumentException();
        }

        private static string FormatUnitBase10(decimal formattedSize, string unit, PrintFormat format)
        {
            if (formattedSize != 1.0M)
            {
                unit += "s";
            }

            if (format == PrintFormat.Smart || format == PrintFormat.Abbreviated)
            {
                return unit[0] + "B";
            }
            else if (format == PrintFormat.Full)
            {
                return unit;
            }
            else if (format == PrintFormat.FullLowerCase)
            {
                return unit.ToLowerInvariant();
            }
            else if (format == PrintFormat.AbbreviatedLowerCase)
            {
                return char.ToLowerInvariant(unit[0]) + "b";
            }

            throw new ArgumentException();
        }

        // Backwards compatibility stuff below

        [Obsolete("Use the correctly-spelled Kibibyte constant instead!")]
        public const long Kebibyte = Kibibyte;
    }
}
