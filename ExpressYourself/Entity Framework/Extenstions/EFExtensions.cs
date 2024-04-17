using ExpressYourself.Entity_Framework.Types;
using ExpressYourself.Types;

namespace ExpressYourself.Entity_Framework.Extenstions
{
    public static class EFExtensions
    {
        public static CountryInfo ToCountryInfo(this Countries country)
        {
            return new CountryInfo
            {
                CountryName = country.Name,
                ThreeLetterCode = country.ThreeLetterCode,
                TwoLetterCode = country.TwoLetterCode
            };
        }

        public static Countries ToCountries(this CountryInfo countryInfo)
        {
            return new Countries
            {
                Name = countryInfo.CountryName,
                ThreeLetterCode = countryInfo.ThreeLetterCode,
                TwoLetterCode = countryInfo.TwoLetterCode
            };
        }
    }
}
