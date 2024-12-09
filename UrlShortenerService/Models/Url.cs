namespace UrlShortenerService.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }  // foreign key linking to User collection
    }
}
