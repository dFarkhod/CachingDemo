using Microsoft.EntityFrameworkCore;
using VirtualDars.CachingDemo.Entities;

namespace VirtualDars.CachingDemo.Infra
{
    public class CountryDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }
        public CountryDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
