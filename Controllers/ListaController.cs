using KanabanBack.Models;
using KanabanBack.Models.DTOs.Board;
using KanabanBack.Models.DTOs.card;
using KanabanBack.Models.DTOs.List;
using KanabanBack.Models.DTOs.Tag;
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
        [HttpPost]
        [Route("CreateLista")]
        public async Task<IActionResult> CreateLista([FromBody] ListaDtoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var boardExists = await _newkanbanContext.Boards.AnyAsync(b => b.BoardId == request.BoardId);
                if (!boardExists)
                    return BadRequest("El BoardId no existe.");
                var newLista = new List
                {
                    Nombre = request.Nombre,
                    BoardId = request.BoardId,
                    Order = 0

                };
                _newkanbanContext.Add(newLista);
                await _newkanbanContext.SaveChangesAsync();

                var response = new ListaDtoResponse
                {
                    ListId = newLista.ListId,
                    Nombre = newLista.Nombre,
                    Order = newLista.Order,
                    BoardId = newLista.BoardId,
                    Cards = newLista.Cards.Select(c => new CardDtoResponse
                    {
                        CardId = c.CardId,
                        Title = c.Title,
                        Descripcion = c.Descripcion,
                        ListId = c.ListId,
                        Tag = c.Tag == null ? null : new TagDtoResponse
                        {
                            Id = c.Tag.Id,
                            Nombre = c.Tag.Nombre
                        }
                    }).ToList()
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor: " + ex.ToString());

            }
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
        [HttpDelete]
        [Route("DeleteLista/{id}")]
        public async Task<IActionResult> DeleteLista(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newLista = await _newkanbanContext.Lists
                    .Include(c => c.Cards)
                    .ThenInclude(t => t.Tag)
                    .FirstOrDefaultAsync(lista => lista.ListId == id);

                if (newLista == null)
                {
                    return NotFound();
                }
                _newkanbanContext.Cards.RemoveRange(newLista.Cards);

                _newkanbanContext.Lists.Remove(newLista);
                await _newkanbanContext.SaveChangesAsync();

                return Ok(new { messege = "Lista Eliminada Correctamente" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor" + ex.ToString());

            }

        }

        
    }
}
