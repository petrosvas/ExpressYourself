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
    }
}
