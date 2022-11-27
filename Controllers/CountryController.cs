using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using VirtualDars.CachingDemo.Entities;
using System.Text.Json;
using VirtualDars.CachingDemo.Infra;
using VirtualDars.CachingDemo.Services;

namespace VirtualDars.CachingDemo
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly ICountryService _countryService;
        public CountryController(IDistributedCache cache, ICountryService countryService)
        {
            this._cache = cache;
            this._countryService = countryService;
        }

        // GET: api/<CountryController>  
        [HttpGet]
        public async Task<IEnumerable<Country>> GetCountries()
        {
            var countriesCache = await _cache.GetStringAsync("countries");
            var value = (countriesCache == null) ? default
                : JsonSerializer.Deserialize<IEnumerable<Country>>(countriesCache);
            if (value == null)
            {
                var countries = await _countryService.GetAllCountriesAsync();
                if (countries != null && countries.Any())
                {
                    await _cache.SetStringAsync("countries", JsonSerializer.Serialize(countries), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                    return countries;
                }
            }
            return value;
        }


        [HttpPost]
        public async Task<ActionResult<string>> AddCountry([FromBody] Country country, CancellationToken cancellationToken)
        {
            if (country == null)
                return BadRequest("country is null");

            await _countryService.CreateCountry(country);
            await _cache.RemoveAsync("countries", cancellationToken).ConfigureAwait(false);
            return Ok("cache has been invalidated");
        }
    }
}
