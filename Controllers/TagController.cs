using KanabanBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanabanBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly NewkanbanContext _newkanbanContext;
        public TagController(NewkanbanContext newkanbanContext)
        {
            _newkanbanContext = newkanbanContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var Tags = await _newkanbanContext.Tags.ToListAsync();
            if (Tags == null) return NotFound();
            return Ok(Tags);
        }
    }
}
