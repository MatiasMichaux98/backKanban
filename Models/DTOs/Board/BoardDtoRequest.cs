namespace KanabanBack.Models.DTOs.Board
{
    public class BoardDtoRequest
    {
        public string Nombre { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        //public int? UsuarioId { get; set; }

    }
}
