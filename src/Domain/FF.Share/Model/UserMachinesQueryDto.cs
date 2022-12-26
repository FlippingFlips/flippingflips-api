namespace FF.Shared.Model
{
    public class UserMachinesQueryDto
    {
        public int Limit { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int Pages { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public bool IncludePlayers { get; set; }
    }
}
