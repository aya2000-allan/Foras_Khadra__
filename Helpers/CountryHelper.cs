using Nager.Country;
using System.Collections.Generic;
using System.Linq;

namespace Foras_Khadra.Helpers
{
    public class CountryHelper
    {
        public static List<string> GetAllCountries()
        {
            var provider = new CountryProvider();
            return provider.GetCountries()
                           .Select(c => c.CommonName)
                           .OrderBy(c => c)
                           .ToList();
        }
    }
}
