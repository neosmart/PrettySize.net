using System;

namespace NeoSmart.PrettySize
{
    public struct PrettySize : IFormattable
    {
        public const long Kilobyte = 1000;        
        public const long Megabyte = 1000 * Kilobyte;        
        public const long Gigabyte = 1000 * Megabyte;
        public const long Terabyte = 1000 * Gigabyte;
        public const long Petabyte = 1000 * Terabyte;
        public const long Exabyte  = 1000 * Petabyte;

        public const long Kebibyte = 1 << 10;
        public const long Mebibyte = 1 << 20;
        public const long Gibibyte = 1 << 30;
        public const long Tebibyte = 1 << 40;
        public const long Pebibyte = 1 << 50;
        public const long Exbibyte = 1 << 60;

        public const long KB = Kilobyte;
        public const long KiB = Kebibyte;
        public const long MB = Megabyte;
        public const long MiB = Mebibyte;
        public const long GB = Gigabyte;
        public const long GiB = Gibibyte;
        public const long TB = Terabyte;
        public const long TiB = Tebibyte;

        delegate string FormatDelegate(ulong size, CalculationBase @base, PrintFormat format);

        struct FormattingRule : IComparable
        {
            public ulong LessThan;
            public FormatDelegate FormatDelegate;

            public int CompareTo(object other)
            {
                //we are assuming other is always IComparable to avoid overhead of "is ulong" check
                //Array.BinarySearch always used the object's ICompare even if an IComparer was specified until .NET 4.5
                //https://stackoverflow.com/a/19319601/17027
                return LessThan.CompareTo((ulong)other);
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
            new FormattingRule { LessThan = 0, FormatDelegate = null }, //this should never be reached
            new FormattingRule { LessThan = 1 * Kilobyte, FormatDelegate = (size, @base, format) =>
            {
                return PrintBytes(size, format);
            } },
            new FormattingRule { LessThan = 10 * Kilobyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kilobyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Kilobyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kilobyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Megabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kilobyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Kilobyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Megabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Megabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Megabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Megabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Gigabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Megabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Megabyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Gigabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gigabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Gigabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gigabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Terabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gigabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Gigabyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Terabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Terabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Terabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Terabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Petabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Terabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Terabyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Petabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Petabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Petabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Petabyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Exabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Petabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Petabyte", format)}";
            } },
            new FormattingRule { LessThan = 10L * (ulong)Exabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exabyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Exabyte", format)}";
            } },
            new FormattingRule { LessThan = ulong.MaxValue, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exabyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Exabyte", format)}";
            } },
        };

        static private readonly FormattingRule[] Base2Map = new FormattingRule[]
        {
            new FormattingRule { LessThan = 0, FormatDelegate = null }, //this should never be reached
            new FormattingRule { LessThan = 1 * Kilobyte, FormatDelegate = (size, @base, format) =>
            {
                return PrintBytes(size, format);
            } },
            new FormattingRule { LessThan = 10 * Kilobyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kebibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Kebibyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Kilobyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kebibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Kebibyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Megabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Kebibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Kebibyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Megabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Mebibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Megabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Mebibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Gigabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Mebibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Mebibyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Gigabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gibibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Gigabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gibibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Terabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Gibibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Gibibyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Terabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Tebibyte));
                return $"{formattedSize:N2} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Terabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Tebibyte));
                return $"{formattedSize:N1} {FormatUnitBase2(formattedSize, "Tebibyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Petabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Tebibyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Tebibyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * Petabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Pebibyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Pebibyte", format)}";
            } },
            new FormattingRule { LessThan = 100 * Petabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Pebibyte));
                return $"{formattedSize:N1} {FormatUnitBase10(formattedSize, "Pebibyte", format)}";
            } },
            new FormattingRule { LessThan = 1 * Exabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Pebibyte));
                return $"{formattedSize:N0} {FormatUnitBase10(formattedSize, "Pebibyte", format)}";
            } },
            new FormattingRule { LessThan = 10 * (ulong)Exabyte, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exbibyte));
                return $"{formattedSize:N2} {FormatUnitBase10(formattedSize, "Exbibyte", format)}";
            } },
            new FormattingRule { LessThan = ulong.MaxValue, FormatDelegate = (size, @base, format) =>
            {
                var formattedSize = (size / (1M * Exbibyte));
                return $"{formattedSize:N0} {FormatUnitBase2(formattedSize, "Exbibyte", format)}";
            } },
        };

        public readonly long Bytes;

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
            throw new NotImplementedException();
        }

        public static string Format(long size, CalculationBase @base = CalculationBase.Base2, PrintFormat format = PrintFormat.Smart)
        {
            return Format((ulong)size, @base, format);
        }

        public static string Format(ulong size, CalculationBase @base = CalculationBase.Base2, PrintFormat format = PrintFormat.Smart)
        {
            if (@base == CalculationBase.Base2)
            {
                var searchIndex = Array.BinarySearch(Base2Map, size);
                if (searchIndex < 0)
                {
                    searchIndex = ~searchIndex;
                    return Base2Map[searchIndex].FormatDelegate(size, @base, format);
                }
                return Base2Map[searchIndex + 1].FormatDelegate(size, @base, format);
            }
            else
            {
                var searchIndex = Array.BinarySearch(Base10Map, size);
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
                return unit.ToLower();
            }
            else if (format == PrintFormat.AbbreviatedLowerCase)
            {
                return Char.ToLower(unit[0]) + "ib";
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
                return unit.ToLower();
            }
            else if (format == PrintFormat.AbbreviatedLowerCase)
            {
                return Char.ToLower(unit[0]) + "b";
            }

            throw new ArgumentException();
        }
    }
}
