namespace ExpressYourself.Entity_Framework.Types
{
    public class IPAddresses
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string IP { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
