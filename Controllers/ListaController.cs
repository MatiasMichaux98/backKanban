using KanabanBack.Models;
using KanabanBack.Models.DTOs.Board;
using KanabanBack.Models.DTOs.List;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanabanBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListaController : ControllerBase
    {
        private readonly NewkanbanContext _newkanbanContext;

        public ListaController(NewkanbanContext newkanbanContext)
        {
            _newkanbanContext = newkanbanContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetListas()
        {
            var listas = await _newkanbanContext.Lists.ToListAsync();
            if (listas == null) return NotFound();
            return Ok(listas);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetListaId(int id)
        {
            var lista = await _newkanbanContext.Lists.FindAsync(id);
            if (lista == null)
            {
                return NotFound(new { messege = "lista no Encontrada" });
            }
            return Ok(lista);
        }


        [HttpPut]
        [Route("UpdateLista/{id}")]
        public async Task<IActionResult> UpdateLista(BoardDtoRequest request, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var UpdateLista = await _newkanbanContext.Lists.FindAsync(id);
                if (UpdateLista == null)
                {
                    return NotFound(new { meseege = "lista no encontrada" });
                }

                UpdateLista.Nombre = request.Nombre;

                _newkanbanContext.Entry(UpdateLista).State = EntityState.Modified;
                await _newkanbanContext.SaveChangesAsync();

                var response = new ListaDtoResponse
                {
                    ListId = UpdateLista.ListId,
                    BoardId = UpdateLista.BoardId,
                    Nombre = UpdateLista.Nombre,
                    Order = UpdateLista.Order
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
