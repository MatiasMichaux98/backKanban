using KanabanBack.Models.DTOs.Tag;

namespace KanabanBack.Models.DTOs.card
{
    public class CardDtoResponse
    {
        public int CardId { get; set; }
        public string? Title { get; set; }
        public int? ListId { get; set; }
        public string? Descripcion { get; set; }
        public TagDtoResponse? Tag { get; set; }




    }
}
