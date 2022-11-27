using VirtualDars.CachingDemo.Entities;
using VirtualDars.CachingDemo.Infra;

namespace VirtualDars.CachingDemo.Services
{
    public class CountryService : ICountryService
    {
        private CountryRepository _countryRepo;

        public CountryService(CountryRepository countryRepo)
        {
            _countryRepo = countryRepo;
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            return await _countryRepo.GetAll();
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            return await _countryRepo.Get(id);
        }

        public async Task<Country> CreateCountry(Country country)
        {
            return await _countryRepo.Add(country);
        }
    }
}
