using Microsoft.AspNetCore.Mvc;
using VirtualDars.CachingDemo.Entities;
using VirtualDars.CachingDemo.Services;

namespace VirtualDars.CachingDemo
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountryController(ICountryService countryService)
        {
            this._countryService = countryService;
        }

        // GET: api/<CountryController>  
        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            return Ok(await _countryService.GetAllCountriesAsync());
        }


        [HttpPost]
        public async Task<IActionResult> AddCountry([FromBody] Country country, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _countryService.CreateCountry(country);
            return CreatedAtAction(nameof(AddCountry), new { id = country.Id }, country);
        }
    }
}
