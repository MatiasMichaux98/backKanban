using KanabanBack.Models;
using KanabanBack.Models.Custom;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KanabanBack.Service
{
    public class AutorizacionService: IAutorizacionService
    {
        public readonly IConfiguration _configuration;
        public readonly NewkanbanContext _kanbanContext;

        public AutorizacionService(NewkanbanContext kanbanContext, IConfiguration configuration)
        {
            _kanbanContext = kanbanContext;
            _configuration = configuration;
        }


        private string GenerateToken(string IdUsuario)
        {
            var key = _configuration.GetValue<string>("JwtSettings:Key");
            var KeyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, IdUsuario));

            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(KeyBytes),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(20)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string tokenCreado = tokenHandler.WriteToken(tokenConfig);
            return tokenCreado;
        }

        private string GenerateRefreshToken()
        {
            var bytesArray = new byte[64];
            using(var mg = RandomNumberGenerator.Create())
            {
                mg.GetBytes(bytesArray);
            }
            return Convert.ToBase64String(bytesArray);
        }

        public async Task<AutorizacionResponse>GuardarHistorialTokenRefresh(int Id , string token, string refreshtoken,string nombreUsuario)
        {
            var historialRefreshToken = new HistorialRefreshToken
            {
                IdUsuario = Id,
                Token = token,
                RefreshToken = refreshtoken,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddDays(7)
            };
            await _kanbanContext.HistorialRefreshTokens.AddAsync(historialRefreshToken);
            await _kanbanContext.SaveChangesAsync();
            return new AutorizacionResponse
            {
                UsuarioId = Id,
                Token = token,
                RefreshToken = refreshtoken,
                Resultado = true,
                Msg = "ok",
                NombreUsuario = nombreUsuario 
            };
        }
        
        public async Task<AutorizacionResponse> DevolverToken(AutorizacionRequest request)
        {
            var usuarioEncontrado = _kanbanContext.Users.FirstOrDefault(x =>
            x.NombreUsuario == request.NombreUsuario &&
            x.Clave == request.Clave);

            if (usuarioEncontrado == null) return await Task.FromResult<AutorizacionResponse>(null);
            string tokenCreado = GenerateToken(usuarioEncontrado.Id.ToString());
            var refreshTokenCreado = GenerateRefreshToken();

            return await GuardarHistorialTokenRefresh(usuarioEncontrado.Id, tokenCreado, refreshTokenCreado, usuarioEncontrado.NombreUsuario);
        }
       
     

        public async Task<AutorizacionResponse> DevolverTokenRefresh(RefreshTokenRequest refreshTokenRequest, int Id)
        {
            var refreshTokenEncontrado = _kanbanContext.HistorialRefreshTokens.FirstOrDefault(x =>
                x.Token == refreshTokenRequest.TokenExpire &&
                x.RefreshToken == refreshTokenRequest.RefreshToken &&
                x.IdUsuario == Id

            );

            if (refreshTokenEncontrado == null)
            {
                return new AutorizacionResponse { Resultado = false, Msg = "No existe token" };
            }

            var usuario = await _kanbanContext.Users.FindAsync(Id);
            if(usuario == null)
            {
                return new AutorizacionResponse { Resultado = false, Msg = "Usuario no encontrado" };

            }

            var refreshTokenCreado = GenerateRefreshToken();
            var tokenCreado = GenerateToken(Id.ToString());
            Console.WriteLine($"Nuevo token creado para usuario {Id}: {tokenCreado}");

            return await GuardarHistorialTokenRefresh(Id, tokenCreado, refreshTokenCreado, usuario.NombreUsuario);
        }
    }
}
