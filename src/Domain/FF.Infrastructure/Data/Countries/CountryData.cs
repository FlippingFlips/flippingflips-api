using FF.Domain.Models.Data;
using System.Text.Json;

namespace FF.Infrastructure.Data.Countries
{
    public static class CountryData
    {
        static CountryData()
        {
            var cjson = File.ReadAllText("Data/Countries/ISO.json");
            Countries = JsonSerializer.Deserialize<List<Country>>(cjson);
        }

        public static IEnumerable<Country>? Countries { get; }
    }
}
