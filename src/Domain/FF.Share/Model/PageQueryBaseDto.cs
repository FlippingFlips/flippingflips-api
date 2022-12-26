namespace FF.Shared.Model
{
    public abstract class PageQueryBaseDto
    {
        public int MaxLimit { get; set; } = 50;
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
