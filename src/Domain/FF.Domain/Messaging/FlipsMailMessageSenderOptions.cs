namespace FF.Domain.Messaging
{
    public class FlipsMailMessageSenderOptions
    {
        public string? SendGridUser { get; set; }
        public string? SendGridKey { get; set; }
        public string? EmailFrom { get; set; }
        public string? EmailFromName { get; set; }
        public string? EmailFooterUrl { get; set; }
        public string? SiteName { get; set; }
    }
}