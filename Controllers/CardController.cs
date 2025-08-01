using KanabanBack.Models.DTOs.Board;
using KanabanBack.Models.DTOs.List;
using KanabanBack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KanabanBack.Models.DTOs.card;

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
            var cards = await _newkanbanContext.Cards.ToListAsync();
            if (cards == null) return NotFound();
            return Ok(cards);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCardId(int id)
        {
            var card = await _newkanbanContext.Cards.FindAsync(id);
                
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
                var newCard = new Card
                {
                    Title = request.Title,
                    Descripcion = request.Descripcion,
                    ListId = request.ListId
                    //UsuarioId = request.UsuarioId
                };
                _newkanbanContext.Add(newCard);
                await _newkanbanContext.SaveChangesAsync();

                var response = new CardDtoResponse
                {
                    Title = request.Title,
                    Descripcion = request.Descripcion,
                    ListId = request.ListId,
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor" + ex.Message);
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

                _newkanbanContext.Entry(UpdateCard).State = EntityState.Modified;
                await _newkanbanContext.SaveChangesAsync();

                var response = new CardDtoResponse
                {
                    Title = request.Title,
                    Descripcion = request.Descripcion,
                    ListId = request.ListId,
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
                var cardDelete = await _newkanbanContext.Cards.FindAsync(id);
                    

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

