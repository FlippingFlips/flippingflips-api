namespace FF.Domain.Models
{
    public class CreatePlayerOption
    {
        public string? Initials { get; set; }
        public string? Name { get; set; }
        public bool IsDefault { get; set; }
        public string? UserId { get; set; }
    }
}
