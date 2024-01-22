namespace Vrt.Vivec.Svc.Data.Response
{
    public class News
    {
        public int PageElements { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public List<MessageDTO>? Messages { get; set; }
    }

    public class MessageDTO
    {
        public CategoryDTO? Category { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? ImageURL { get; set; }
    }
}
