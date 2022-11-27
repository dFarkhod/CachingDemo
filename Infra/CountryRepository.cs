using VirtualDars.CachingDemo.Entities;

namespace VirtualDars.CachingDemo.Infra
{
    public class CountryRepository : Repository<Country, CountryDbContext>
    {
        public CountryRepository(CountryDbContext context) : base(context)
        {

        }
    }
}
