using ExpressYourself.Types;

namespace ExpressYourself.Extensions
{
    public static partial class Extensions
    {
        public static CountryInfo ToCountryInfo(this string[] array)
        {
            return new CountryInfo
            {
                CountryName = array.Name(),
                ThreeLetterCode = array.ThreeLetterCode(),
                TwoLetterCode = array.TwoLetterCode()
            };
        }

        public static CountryInfo ToCountryInfo(this Sql_Countries sql_Countries)
        {
            return new CountryInfo
            {
                CountryName = sql_Countries.Name,
                ThreeLetterCode = sql_Countries.ThreeLetterCode,
                TwoLetterCode = sql_Countries.TwoLetterCode
            };
        }
    }
}
