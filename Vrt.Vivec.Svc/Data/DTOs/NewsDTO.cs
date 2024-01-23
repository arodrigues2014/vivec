

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
        public CategoryDTO? Category { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverterHelper))]
        public DateTime PublicationDate { get; set; }
        [JsonConverter(typeof(EpochDateTimeConverterHelper))]
        public DateTime LastModifiedDate { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? ImageURL { get; set; }

    }

    public class NewsHtmlDTO
    {
        public int PageElements { get; set; }
        public int TotalPages { get; set; }
        //public int TotalElements { get; set; }
        public List<MessageHtmlDTO>? Messages { get; set; }
    }
    public class MessageHtmlDTO
    {
        // Represents the category of the message.
        public CategoryDTO? Category { get; set; }

        // Represents the publication date of the message.
        public DateTime PublicationDate { get; set; }

        // Represents the last modified date of the message.
        public DateTime LastModifiedDate { get; set; }

        // Represents the title of the message.
        public string? Title { get; set; }

        // Represents the text content of the message.
        public string? Text { get; set; }

        // Represents the URL of an image associated with the message.
        public string? ImageURL { get; set; }

        /// <summary>
        /// Initializes a new instance of the MessageHtml class.
        /// </summary>
        /// <param name="messageDTO">The source message DTO to initialize from.</param>
        public MessageHtmlDTO(MensajesDTO messageDTO)
        {
            HtmlDocument doc = new HtmlDocument();
            var html = messageDTO.Text;
            // Cargar el HTML en el objeto HtmlDocument
            doc.LoadHtml(html);
            // Map each field from MensajesDTO to MessageHtml
            Category = messageDTO.Category;
            PublicationDate = messageDTO.PublicationDate;
            LastModifiedDate = messageDTO.LastModifiedDate;
            Title = messageDTO.Title;
            Text = doc.DocumentNode.InnerText;
            ImageURL = messageDTO.ImageURL;
        }
    }
}