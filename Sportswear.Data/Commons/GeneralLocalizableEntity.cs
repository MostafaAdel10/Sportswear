using System.Globalization;

namespace Sportswear.DataAccess.Commons
{
    public static class LocalizationExtensions
    {
        public static string Localize(this object _, string textAr, string textEN)
        {
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            if (cultureInfo.TwoLetterISOLanguageName.ToLower().Equals("ar"))
                return textAr;
            return textEN;
        }
    }
}
