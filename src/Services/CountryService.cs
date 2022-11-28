using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using VirtualDars.CachingDemo.Entities;
using VirtualDars.CachingDemo.Infra;

namespace VirtualDars.CachingDemo.Services
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepository _countryRepo;
        private readonly IDistributedCache _cache;

        public CountryService(CountryRepository countryRepo, IDistributedCache cache)
        {
            _countryRepo = countryRepo;
            _cache = cache;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            IEnumerable<Country> countries = null;
            var countriesFromCache = await _cache.GetStringAsync(typeof(Country).FullName);
            countries = (countriesFromCache == null) ? default
                : JsonSerializer.Deserialize<IEnumerable<Country>>(countriesFromCache);

            if (countries == null)
            {
                countries = await _countryRepo.GetAll();
                if (countries != null && countries.Any())
                {
                    await _cache.SetStringAsync(typeof(Country).FullName, JsonSerializer.Serialize(countries), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                }
            }
            return countries;
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            return await _countryRepo.Get(id);
        }

        public async Task<Country> CreateCountry(Country country)
        {
            await _countryRepo.Add(country);
            await _cache.RemoveAsync(typeof(Country).FullName).ConfigureAwait(false);
            return country;
        }
    }
}
