

namespace Vrt.Vivec.Svc.Data.Response
{
    public class NewsResponse
    {
        public int Page { get; set; }
        public int PageElements { get; set; }
        public int TotalPages { get; set; }
        public int TotalElements { get; set; }
        public bool LastPage { get; set; }
        public int TotalUnread { get; set; }
        public List<Message>? Messages { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public Category? Category { get; set; }
        public long PublicationDate { get; set; }
        public int State { get; set; }
        public int ScheduleKind { get; set; }
        public int Population { get; set; }
        public long LastModifiedDate { get; set; }
        public string Language { get; set; }
        public List<string>? AvailableLanguages { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? ImageURL { get; set; }
        public bool Read { get; set; }
        public bool Answered { get; set; }
        public int Kind { get; set; }
        public bool Highlighted { get; set; }
        public bool Shareable { get; set; }
        public double DialengaHappinessScore { get; set; }
        public int TotalVotes { get; set; }
        public bool CommentsEnabled { get; set; }
        public int TotalComments { get; set; }
        public bool AllowImageZooming { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Color { get; set; }
        public object ParentId { get; set; }
        public string Language { get; set; }
        public string ImageURL { get; set; }
    }
}
