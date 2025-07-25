using KanabanBack.Models;
using KanabanBack.Models.Custom;
using KanabanBack.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace KanabanBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly NewkanbanContext _newkanbanContext;
        public readonly IAutorizacionService _autorizacionService;
        public UsuarioController(NewkanbanContext newkanbanContext, IAutorizacionService autorizacionService)
        {
            _newkanbanContext = newkanbanContext;
            _autorizacionService = autorizacionService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return NotFound();
                }
                var nuevoUsuario = new User
                {
                    NombreUsuario = request.NombreUsuario,
                    Clave = request.Clave,
                    Email = request.Email
                };
                await _newkanbanContext.AddAsync(nuevoUsuario);
                await _newkanbanContext.SaveChangesAsync();

                return Ok(new
                {
                    isSuccess = true,
                    Messege = "Usuario nuevo registrado Correctamente!!"
                });

            }catch(Exception ex)
            {
                return StatusCode(500, "Ocurrio un error en el servidor " + ex.Message);
            }
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] AutorizacionRequest request)
        {
            var usuarioEncontrado = await _autorizacionService.DevolverToken(request);
            if(usuarioEncontrado == null)
            {
                return Unauthorized();
            }

            var response = new AutorizacionResponse
            {
                Token = usuarioEncontrado.Token,
                RefreshToken = usuarioEncontrado.RefreshToken,
                Resultado = usuarioEncontrado.Resultado,
                Msg = usuarioEncontrado.Msg,
                UsuarioId = usuarioEncontrado.UsuarioId,
                NombreUsuario = usuarioEncontrado.NombreUsuario
            };

            return Ok(response);

        }

        [HttpPost]
        [Route("ObtenerRefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenExpirado = tokenHandler.ReadJwtToken(request.TokenExpire);
                if (tokenExpirado.ValidTo > DateTime.UtcNow)
                    return BadRequest(new AutorizacionResponse { Resultado = false, Msg = "Token no Expiro" });

                string IdUsuario = tokenExpirado.Claims.First(x =>
                x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

                var autorizacionResponse = await _autorizacionService.DevolverTokenRefresh(request, int.Parse(IdUsuario));

                if (autorizacionResponse.Resultado)
                {
                    return Ok(autorizacionResponse);
                }
                else
                {
                    return BadRequest(autorizacionResponse);
                }

            }catch(Exception ex)
            {
                Console.WriteLine($"[ERROR RefreshToken] {ex.Message}");
                return StatusCode(500, new AutorizacionResponse
                {
                    Resultado = false,
                    Msg = $"Error interno en el refresh token: {ex.Message}"
                });
            }
           
        }
        
    }
}
