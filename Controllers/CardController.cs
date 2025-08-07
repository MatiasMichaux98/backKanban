using KanabanBack.Models.DTOs.Board;
using KanabanBack.Models.DTOs.List;
using KanabanBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KanabanBack.Models.DTOs.card;
using KanabanBack.Models.DTOs.Tag;

namespace KanabanBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly NewkanbanContext _newkanbanContext;
        public CardController(NewkanbanContext newkanbanContext)
        {
            _newkanbanContext = newkanbanContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards()
        {
            var cards = await _newkanbanContext.Cards
                .Include(t => t.Tag)
                .ToListAsync();
            if (cards == null) return NotFound();
            return Ok(cards);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCardId(int id)
        {
            var card = await _newkanbanContext.Cards
                .Include(t => t.Tag)
                .FirstOrDefaultAsync(card => card.CardId == id);
                
            if (card == null)
            {
                return NotFound(new { messege = "card no Encontrada" });
            }

            var response = new CardDtoResponse
            {
                CardId = card.CardId,
                Title = card.Title,
                Descripcion = card.Descripcion,
                ListId = card.ListId,
                Tag = card.Tag == null ? null : new TagDtoResponse
                {
                    Id = card.Tag.Id,
                    Nombre = card.Tag.Nombre
                }
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("CreateCard")]
        public async Task<IActionResult> CreateCard([FromBody] CardDtoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var listExists = await _newkanbanContext.Lists.AnyAsync(l => l.ListId == request.ListId);
                if (!listExists)
                    return BadRequest($"No existe la lista con ListId = {request.ListId}");

                var newCard = new Card
                {
                    Title = request.Title,
                    Descripcion = request.Descripcion,
                    ListId = request.ListId,
                    TagId = request.TagId
                    //UsuarioId = request.UsuarioId
                };
                _newkanbanContext.Add(newCard);
                await _newkanbanContext.SaveChangesAsync();

                var cardWithTag = await _newkanbanContext.Cards
                    .Include(c => c.Tag)
                    .FirstOrDefaultAsync(c => c.CardId == newCard.CardId);
                var response = new CardDtoResponse
                {
                    CardId = newCard.CardId,
                    Title = newCard.Title,
                    Descripcion = newCard.Descripcion,
                    ListId = newCard.ListId,
                    Tag = newCard.Tag == null ? null : new TagDtoResponse
                    {
                        Id = newCard.Tag.Id,
                        Nombre = newCard.Tag.Nombre
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message} - {ex.StackTrace}");

            }
        }

        [HttpPut]
        [Route("UpdateCard/{id}")]
        public async Task<IActionResult> UpdateCard(CardDtoRequest request, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var UpdateCard = await _newkanbanContext.Cards.FindAsync(id);
                if (UpdateCard == null)
                {
                    return NotFound(new { meseege = "card no encontrada" });
                }

                UpdateCard.Title = request.Title;
                UpdateCard.Descripcion = request.Descripcion;
                UpdateCard.TagId = request.TagId;

                _newkanbanContext.Entry(UpdateCard).State = EntityState.Modified;
                await _newkanbanContext.SaveChangesAsync();

                var cardWithTag = await _newkanbanContext.Cards
                    .Include(c => c.Tag)
                    .FirstOrDefaultAsync(c => c.CardId == UpdateCard.CardId);
                var response = new CardDtoResponse
                {
                    Title = cardWithTag.Title,
                    Descripcion = cardWithTag.Descripcion,
                    ListId = cardWithTag.ListId,
                    Tag = cardWithTag.Tag == null ? null : new TagDtoResponse
                    {
                        Id = cardWithTag.Tag.Id,
                        Nombre = cardWithTag.Tag.Nombre
                    }

                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor" + ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteCard/{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var cardDelete = await _newkanbanContext.Cards
                    .Include(t => t.Tag)
                    .FirstOrDefaultAsync(card => card.CardId == id);
                    

                if (cardDelete == null)
                {
                    return NotFound();
                }

                _newkanbanContext.Entry(cardDelete).State = EntityState.Deleted;
                await _newkanbanContext.SaveChangesAsync();

                return Ok(new { messege = "card Eliminada Correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor" + ex.Message);
            }
        }

    }
}

