using VirtualDars.CachingDemo.Entities;

namespace VirtualDars.CachingDemo.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryByIdAsync(int id);
        Task<Country> CreateCountry(Country country);
    }
}
