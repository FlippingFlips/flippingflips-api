namespace FF.Domain.Models { 
    /// <summary>
    /// Model for posting scores to server
    /// </summary>
    public class GamePlayedDto
    {
        public string? Id { get; set; }
        public long P1Score { get; set; }
        public long? P2Score { get; set; }
        public long? P3Score { get; set; }
        public long? P4Score { get; set; }
        public string? NvRamBase64 { get; set; }
        public string? CRC { get; set; }
    }
}