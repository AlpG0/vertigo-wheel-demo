using System.Globalization;

namespace VertigoWheel.Utils
{
    /// <summary>
    /// Ekranda gorunen butun sayilarin formatlandigi tek yer. Format degisecekse (binlik ayirici, K kisaltmasi...) sadece burasi degisir.
    /// </summary>
    public static class NumberFormatter
    {
        private static readonly NumberFormatInfo DisplayFormat = CreateDisplayFormat(); // 10000 -> "10.000" (referanstaki gibi nokta ile)

        private static NumberFormatInfo CreateDisplayFormat()
        {
            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = "."; // cihazin dil ayarina bagli kalmasin diye kulturu elle kuruyoruz
            format.NumberDecimalSeparator = ",";
            return format;
        }

        public static string FormatAmount(int value) // duz miktar: 950, 10.000
        {
            return value.ToString("N0", DisplayFormat);
        }

        public static string FormatMultiplier(int value) // dilim altindaki miktar: x5, x2.000
        {
            return string.Format(GameConstants.Texts.MultiplierFormat, FormatAmount(value));
        }
    }
}
