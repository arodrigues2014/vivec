
namespace Vrt.Vivec.Svc.Data.DTOs
{
    public class NewsDTO
    {
        public int PageElements { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public List<MensajesDTO>? Messages { get; set; }
    }

    public class MensajesDTO
    {
        public int Id { get; set; }
        public CategoryDTO? Category { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverterHelper))]
        public DateTime PublicationDate { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverterHelper))]
        public DateTime LastModifiedDate { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? ImageURL { get; set; }
    }

  
}