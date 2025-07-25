namespace KanabanBack.Models.Custom
{
    public class AutorizacionResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Resultado { get; set; }
        public string Msg { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }

    }
}
