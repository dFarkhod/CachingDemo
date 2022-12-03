using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using VirtualDars.CachingDemo.Services;

namespace VirtualDars.CachingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private readonly ILogger<CountryService> _logger;
        public AnimalController(ILogger<CountryService> logger)
        {
            _logger= logger;
        }

        [HttpGet]
        [OutputCache(Duration = 5)]
        public IActionResult Get()
        {
            _logger.LogInformation("Ma'lumotni Action Metoddan olyapman!");
            var animals = new List<string> { "Ayiq", "Bo'ri", "Tulki", "Quyon" };
            return Ok(animals);
        }
    }
}
