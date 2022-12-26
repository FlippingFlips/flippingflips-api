namespace FF.Core.Models.Dto
{
    public class PagedBaseDto
    {
        public int CurrentPage { get; set; } = 1;
        public int Limit { get; set; } = 25;
        public int Pages { get; set; }
        public int ResultCount { get; set; }

        public int GetSkipCount()
        {
            return CurrentPage > 1 ? Limit * CurrentPage : 0;
        }
        public void UpdatePages() { Pages = (int)Math.Round((decimal)ResultCount / Limit, 0, MidpointRounding.AwayFromZero); }
    }
}
