public class DocumentSizeConverter
    {
        public readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public string SizeSuffix(Int32 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

    }

    public static class UT
    {
        public static Process StartProcess(this ProcessStartInfo psi, bool useShellExecute = true)
        {
            psi.UseShellExecute = useShellExecute;
            return Process.Start(psi);
        }
    }
