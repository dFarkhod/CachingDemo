using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using VirtualDars.CachingDemo.Services;

namespace VirtualDars.CachingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FruitController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 30)]
        public IActionResult Get()
        {
            var fruits = new List<string> { "Anor", "Olma", "Uzum", "Shaftoli" };
            return Ok(fruits);
        }
    }
}
