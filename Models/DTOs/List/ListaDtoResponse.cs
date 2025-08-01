namespace KanabanBack.Models.DTOs.List
{
    public class ListaDtoResponse
    {
        public int ListId { get; set; }
        public string? Nombre { get; set; }
        public int Order { get; set; }
        public int? BoardId { get; set; }
        //public List<CardDTOsResponse> Cards { get; set; } = new();

    }
}
