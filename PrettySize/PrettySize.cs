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
            return FriendlySize((ulong)Bytes);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        public static string Format(long size, CalculationBase @base = CalculationBase.Base2)
        {
            if (@base == CalculationBase.Base2)
            {
                return FriendlySize((ulong)size);
            }
            else
            {
                return FriendlySizeBase10((ulong)size);
            }
        }

        public static string Format(ulong size, CalculationBase @base = CalculationBase.Base2)
        {
            if (@base == CalculationBase.Base2)
            {
                return FriendlySize(size);
            }
            else
            {
                return FriendlySizeBase10(size);
            }
        }

        public static string FriendlySize(ulong size)
        {
            if (size < 1 * (1L << 10))
            {
                return size + " bytes";
            }
            else if (size < 10 * (1L << 10))
            {
                return (size / (1.0 * (1L << 10))).ToString("N2") + " KiB";
            }
            else if (size < 100 * (1L << 10))
            {
                return (size / (1.0 * (1L << 10))).ToString("N1") + " KiB";
            }
            else if (size < 1 * (1L << 20))
            {
                return (size / (1.0 * (1L << 10))).ToString("N0") + " KiB";
            }
            else if (size < 10 * (1L << 20))
            {
                return (size / (1.0 * (1L << 20))).ToString("N2") + " MiB";
            }
            else if (size < 100 * (1L << 20))
            {
                return (size / (1.0 * (1L << 20))).ToString("N1") + " MiB";
            }
            else if (size < 1 * (1L << 30))
            {
                return (size / (1.0 * (1L << 20))).ToString("N0") + " MiB";
            }
            else if (size < 10 * (1L << 30))
            {
                return (size / (1.0 * (1L << 30))).ToString("N2") + " GiB";
            }
            else if (size < 100 * (1L << 30))
            {
                return (size / (1.0 * (1L << 30))).ToString("N1") + " GiB";
            }
            else if (size < 1 * (1L << 40))
            {
                return (size / (1.0 * (1L << 30))).ToString("N0") + " GiB";
            }
            else if (size < 10 * (1L << 40))
            {
                return (size / (1.0 * (1L << 40))).ToString("N2") + " TiB";
            }
            else if (size < 100 * (1L << 40))
            {
                return (size / (1.0 * (1L << 40))).ToString("N1") + " TiB";
            }
            else
            {
                return (size / (1.0 * (1L << 40))).ToString("N0") + " TiB";
            }
        }

        public static string FriendlySizeBase10(ulong size)
        {
            if (size < 1 * Math.Pow(10, 10))
            {
                return size + " bytes";
            }
            else if (size < 10 * Math.Pow(10, 10))
            {
                return (size / (1.0 * Math.Pow(10, 10))).ToString("N2") + " KiB";
            }
            else if (size < 100 * Math.Pow(10, 10))
            {
                return (size / (1.0 * Math.Pow(10, 10))).ToString("N1") + " KiB";
            }
            else if (size < 1 * Math.Pow(10, 20))
            {
                return (size / (1.0 * Math.Pow(10, 10))).ToString("N0") + " KiB";
            }
            else if (size < 10 * Math.Pow(10, 20))
            {
                return (size / (1.0 * Math.Pow(10, 20))).ToString("N2") + " MiB";
            }
            else if (size < 100 * Math.Pow(10, 20))
            {
                return (size / (1.0 * Math.Pow(10, 20))).ToString("N1") + " MiB";
            }
            else if (size < 1 * Math.Pow(10, 30))
            {
                return (size / (1.0 * Math.Pow(10, 20))).ToString("N0") + " MiB";
            }
            else if (size < 10 * Math.Pow(10, 30))
            {
                return (size / (1.0 * Math.Pow(10, 30))).ToString("N2") + " GiB";
            }
            else if (size < 100 * Math.Pow(10, 30))
            {
                return (size / (1.0 * Math.Pow(10, 30))).ToString("N1") + " GiB";
            }
            else if (size < 1 * Math.Pow(10, 40))
            {
                return (size / (1.0 * Math.Pow(10, 30))).ToString("N0") + " GiB";
            }
            else if (size < 10 * Math.Pow(10, 40))
            {
                return (size / (1.0 * Math.Pow(10, 40))).ToString("N2") + " TiB";
            }
            else if (size < 100 * Math.Pow(10, 40))
            {
                return (size / (1.0 * Math.Pow(10, 40))).ToString("N1") + " TiB";
            }
            else
            {
                return (size / (1.0 * Math.Pow(10, 40))).ToString("N0") + " TiB";
            }
        }
    }
}
