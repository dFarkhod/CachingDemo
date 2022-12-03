using Microsoft.Extensions.Caching.Distributed;
using System.Reflection;
using System.Text.Json;
using VirtualDars.CachingDemo.Entities;
using VirtualDars.CachingDemo.Infra;

namespace VirtualDars.CachingDemo.Services
{
    public class CountryService : ICountryService
    {
        private readonly CountryRepository _countryRepo;
        private readonly IDistributedCache _cache;
        private readonly ILogger<CountryService> _logger;

        public CountryService(CountryRepository countryRepo, IDistributedCache cache, ILogger<CountryService> logger)
        {
            _countryRepo = countryRepo;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var countries = await GetFromCache();
            if (countries == null)
            {
                countries = await _countryRepo.GetAll();
                if (countries != null && countries.Any())
                {
                    await SaveToCache(countries);
                }
            }
            return countries;
        }

        private async Task SaveToCache(IEnumerable<Country> countries)
        {
            try
            {
                await _cache.SetStringAsync(typeof(Country).FullName, JsonSerializer.Serialize(countries), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,  $"Exception occurred in {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }

        private async Task<IEnumerable<Country>> GetFromCache()
        {
            IEnumerable<Country> countries = null;
            try
            {
                var countriesFromCache = await _cache.GetStringAsync(typeof(Country).FullName);
                countries = (countriesFromCache == null) ? null
                    : JsonSerializer.Deserialize<IEnumerable<Country>>(countriesFromCache);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred in {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
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
            // cache'ni bekor qilamiz (invalidation) chunki bazaga yangi ma'umot qo'shildi:
            await InvalidateTheCache();
            return country;
        }

        private async Task InvalidateTheCache()
        {
            try
            {
                await _cache.RemoveAsync(typeof(Country).FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred in {MethodBase.GetCurrentMethod().Name}: {ex.Message}");
            }
        }
    }
}
