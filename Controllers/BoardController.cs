using KanabanBack.Models;
using KanabanBack.Models.DTOs.Board;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanabanBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly NewkanbanContext _newkanbanContext;
        public BoardController(NewkanbanContext newkanbanContext)
        {
            _newkanbanContext = newkanbanContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetBoards()
        {
            var boards = await _newkanbanContext.Boards.ToListAsync();
            if (boards == null) return NotFound();
            return Ok(boards);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetBoardId (int id)
        {
            var board = await _newkanbanContext.Boards.FindAsync(id);
            if(board == null)
            {
                return NotFound(new { messege = "board no Encontrado" });
            }
            return Ok(board);
        }

        [HttpPost]
        [Route("CreateBoard")]
        public async Task<IActionResult> CreateBoard([FromBody] BoardDtoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var newBoard = new Board
                {
                    Nombre = request.Nombre,
                    CreatedAt = DateTime.UtcNow,
                    //UsuarioId = request.UsuarioId
                };
                _newkanbanContext.Add(newBoard);
                await _newkanbanContext.SaveChangesAsync();

                var lista = new List<List>
                {
                    new List {Nombre = "Por Hacer", BoardId = newBoard.BoardId, Order = 0},
                    new List {Nombre = "En proceso", BoardId = newBoard.BoardId, Order = 1},
                    new List {Nombre = "Hecho", BoardId = newBoard.BoardId, Order = 2}
                };
                _newkanbanContext.AddRange(lista);
                await _newkanbanContext.SaveChangesAsync();

                var response = new BoardDtoResponse
                {
                    BoardId = newBoard.BoardId,
                    Nombre = newBoard.Nombre,
                    CreatedAt = DateTime.UtcNow,
                  //  UsuarioId = newBoard.UsuarioId
                };

                return Ok(response);

            }catch(Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor" + ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateBoard/{id}")]
        public async Task<IActionResult> UpdateBoard(BoardDtoRequest request,int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var UpdateBoard = await _newkanbanContext.Boards.FindAsync(id);
                if (UpdateBoard == null)
                {
                    return NotFound(new { meseege = "Usuario no encontrado" });
                }

                UpdateBoard.Nombre = request.Nombre;

                _newkanbanContext.Entry(UpdateBoard).State = EntityState.Modified;
                await _newkanbanContext.SaveChangesAsync();

                var response = new BoardDtoResponse
                {
                    BoardId = UpdateBoard.BoardId,
                    CreatedAt = UpdateBoard.CreatedAt,
                    Nombre = UpdateBoard.Nombre,
                    UsuarioId = UpdateBoard.UsuarioId
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor"+ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteBoard/{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var boardDelete = await _newkanbanContext.Boards.FindAsync(id);
                if (boardDelete == null)
                {
                    return NotFound();
                }
                _newkanbanContext.Entry(boardDelete).State = EntityState.Deleted;
                await _newkanbanContext.SaveChangesAsync();

                return Ok(new { messege = "Board Eliminado Correctamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor" + ex.Message);
            }
        }

    }
}
