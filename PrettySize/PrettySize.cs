using System;

namespace NeoSmart.PrettySize
{
    public struct PrettySize : IFormattable
    {
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
            return FriendlySize((ulong)Bytes, PrintFormat.Abbreviated);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public static string Format(long size, CalculationBase @base = CalculationBase.Base2, PrintFormat format = PrintFormat.Abbreviated)
        {
            if (@base == CalculationBase.Base2)
            {
                return FriendlySize((ulong)size, format);
            }
            else
            {
                return FriendlySizeBase10((ulong)size, format);
            }
        }

        public static string Format(ulong size, CalculationBase @base = CalculationBase.Base2, PrintFormat format = PrintFormat.Abbreviated)
        {
            if (@base == CalculationBase.Base2)
            {
                return FriendlySize(size, format);
            }
            else
            {
                return FriendlySizeBase10(size, format);
            }
        }

        private static string FormatUnitBase2(decimal formattedSize, string unit, PrintFormat format)
        {
            if (formattedSize != 1.0M)
            {
                unit += "s";
            }

            if (format == PrintFormat.Full)
            {
                return unit;
            }
            else if (format == PrintFormat.FullLowerCase)
            {
                return unit.ToLower();
            }
            else if (format == PrintFormat.Abbreviated)
            {
                return unit[0] + "iB";
            }
            else if (format == PrintFormat.AbbreviatedLowerCase)
            {
                return Char.ToLower(unit[0]) + "ib";
            }

            throw new NotImplementedException();
        }

        private static string FormatUnitBase10(decimal formattedSize, string unit, PrintFormat format)
        {
            if (formattedSize != 1.0M)
            {
                unit += "s";
            }

            if (format == PrintFormat.Full)
            {
                return unit;
            }
            else if (format == PrintFormat.FullLowerCase)
            {
                return unit.ToLower();
            }
            else if (format == PrintFormat.Abbreviated)
            {
                return unit[0] + "B";
            }
            else if (format == PrintFormat.AbbreviatedLowerCase)
            {
                return Char.ToLower(unit[0]) + "b";
            }

            throw new NotImplementedException();
        }

        public static string FriendlySize(ulong size, PrintFormat format)
        {
            if (size < 1 * (1L << 10))
            {
                return size + " Bytes";
            }
            else if (size < 10 * (1L << 10))
            {
                var formattedSize = (size / (1.0M * (1L << 10)));
                return formattedSize.ToString("N2") + " " + FormatUnitBase2(formattedSize, "Kibibyte", format);
            }
            else if (size < 1.0M * (1L << 10))
            {
                var formattedSize = (size / (1.0M * (1L << 10)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase2(formattedSize, "Kibibyte", format);
            }
            else if (size < 1 * (1L << 20))
            {
                var formattedSize = (size / (1.0M * (1L << 10)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase2(formattedSize, "Kibibyte", format);
            }
            else if (size < 10 * (1L << 20))
            {
                var formattedSize = (size / (1.0M * (1L << 20)));
				return formattedSize.ToString("N2") + " " + FormatUnitBase2(formattedSize, "Mebibyte", format);
            }
            else if (size < 1.0M * (1L << 20))
            {
                var formattedSize = (size / (1.0M * (1L << 20)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase2(formattedSize, "Mebibyte", format);
            }
            else if (size < 1 * (1L << 30))
            {
                var formattedSize = (size / (1.0M * (1L << 20)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase2(formattedSize, "Mebibyte", format);
            }
            else if (size < 10 * (1L << 30))
            {
                var formattedSize = (size / (1.0M * (1L << 30)));
                return formattedSize.ToString("N2") + " " + FormatUnitBase2(formattedSize, "Gibibyte", format);
            }
            else if (size < 1.0M * (1L << 30))
            {
                var formattedSize = (size / (1.0M * (1L << 30)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase2(formattedSize, "Gibibyte", format);
            }
            else if (size < 1 * (1L << 40))
            {
                var formattedSize = (size / (1.0M * (1L << 30)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase2(formattedSize, "Gibibyte", format);
            }
            else if (size < 10 * (1L << 40))
            {
                var formattedSize = (size / (1.0M * (1L << 40)));
				return formattedSize.ToString("N2") + " " + FormatUnitBase2(formattedSize, "Tebibyte", format);
            }
            else if (size < 1.0M * (1L << 40))
            {
                var formattedSize = (size / (1.0M * (1L << 40)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase2(formattedSize, "Tebibyte", format);
            }
            else
            {
                var formattedSize = (size / (1.0M * (1L << 40)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase2(formattedSize, "Tebibyte", format);
            }
        }

        public static string FriendlySizeBase10(ulong size, PrintFormat format)
        {
            if (size < 1 * Math.Pow(10, 10))
            {
                return size + " Bytes";
            }
            else if (size < 10 * Math.Pow(10, 10))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 10)));
				return formattedSize.ToString("N2") + " " + FormatUnitBase10(formattedSize, "Kilobytes", format);
            }
            else if (size < 1.0M * (decimal)Math.Pow(10, 10))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 10)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase10(formattedSize, "Kilobytes", format);
            }
            else if (size < 1 * Math.Pow(10, 20))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 10)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase10(formattedSize, "Kilobytes", format);
            }
            else if (size < 10 * Math.Pow(10, 20))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 20)));
				return formattedSize.ToString("N2") + " " + FormatUnitBase10(formattedSize, "Megabytes", format);
            }
            else if (size < 1.0M * (decimal)Math.Pow(10, 20))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 20)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase10(formattedSize, "Megabytes", format);
            }
            else if (size < 1 * Math.Pow(10, 30))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 20)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase10(formattedSize, "Megabytes", format);
            }
            else if (size < 10 * Math.Pow(10, 30))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 30)));
				return formattedSize.ToString("N2") + " " + FormatUnitBase10(formattedSize, "Gigabytes", format);
            }
            else if (size < 1.0M * (decimal)Math.Pow(10, 30))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 30)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase10(formattedSize, "Gigabytes", format);
            }
            else if (size < 1 * Math.Pow(10, 40))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 30)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase10(formattedSize, "Gigabytes", format);
            }
            else if (size < 10 * Math.Pow(10, 40))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 40)));
				return formattedSize.ToString("N2") + " " + FormatUnitBase10(formattedSize, "Terabytes", format);
            }
            else if (size < 1.0M * (decimal)Math.Pow(10, 40))
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 40)));
				return formattedSize.ToString("N1") + " " + FormatUnitBase10(formattedSize, "Terabytes", format);
            }
            else
            {
                var formattedSize = (size / (1.0M * (decimal)Math.Pow(10, 40)));
				return formattedSize.ToString("N0") + " " + FormatUnitBase10(formattedSize, "Terabytes", format);
            }
        }
    }
}
