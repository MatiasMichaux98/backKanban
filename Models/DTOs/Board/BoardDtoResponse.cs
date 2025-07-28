namespace KanabanBack.Models.DTOs.Board
{
    public class BoardDtoResponse
    {
        public int BoardId { get; set; }

        public string Nombre { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public int? UsuarioId { get; set; }

    }
}
