using KanabanBack.Models;
using KanabanBack.Models.DTOs.Board;
using KanabanBack.Models.DTOs.Tag;
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

        [HttpPut]
        [Route("UpdateTag/{id}")]
        public async Task<IActionResult> Update(TagDtoRequest request, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var UpdateTag = await _newkanbanContext.Tags.FindAsync(id);

                if (UpdateTag == null)
                {
                    return NotFound(new { meseege = "board no encontrado" });
                }

                UpdateTag.Nombre = request.Nombre;

                _newkanbanContext.Entry(UpdateTag).State = EntityState.Modified;
                await _newkanbanContext.SaveChangesAsync();

                var response = new TagDtoResponse
                {
                    Id = UpdateTag.Id,
                    Nombre = UpdateTag.Nombre
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor" + ex.Message);
            }
        }
    }
}
