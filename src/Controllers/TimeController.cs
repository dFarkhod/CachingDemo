using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace VirtualDars.CachingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        // Farhod: Ogohlantirish! IMemoryCache'da concurrency bilan bog'liq muammolari borligi sababli
        // uni o'rniga LazyCache nuget'ni ishlatilishi tavsiya etaman!

        private readonly IMemoryCache _memoryCache;
        private const string CACHE_KEY = "CurrentTime";
        public TimeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

        }

        [HttpGet]
        public IActionResult GetTime()
        {
            DateTime currentDateTime = DateTime.Now;

            // 1-Uslub: Cache'dan o'qish TryGetValue() va yozish Set() alohida yoziladi
            // (double locking cache pattern):
            bool cacheHit = _memoryCache.TryGetValue(CACHE_KEY, out DateTime cacheValue);
            if (!cacheHit)
            {
                cacheValue = currentDateTime;
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
                  //.SetSlidingExpiration(TimeSpan.FromSeconds(3));

                _memoryCache.Set(CACHE_KEY, cacheValue, cacheEntryOptions);
            }
            else
            {
                currentDateTime = cacheValue;
            }

            /* !!! Ogohlantirish: 
             Sliding expiration'ni o'zini ishlatilgan holatda agar o'sha ma'lumot serverdan tez-tez olinsa,
             u ma'lumotning yaroqlilik muddati tugamaslik havfi bor.
             Shuning uchun SlidingExpiration bilan birgalikda AbsoluteExpiration ham ishlatilishi kk!
             */

            // 2-Uslub: Cache'dan o'qish va yozish bittada yoziladi: GetOrCreate()
            /* currentDateTime = _memoryCache.GetOrCreate(CACHE_KEY,
                                cacheEntry =>
                                {
                                    cacheEntry.SlidingExpiration = TimeSpan.FromSeconds(3);
                                    cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
                                    return DateTime.Now;
                                });*/

            return Ok(currentDateTime);
        }
    }
}
