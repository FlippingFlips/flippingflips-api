namespace FF.Core.Models
{
    /// <summary>
    /// For user to set up a game
    /// </summary>
    public class GameInProgress
    {
        public string Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string GameId { get; set; }
        public int Player1Id { get; set; }
        public int? Player2Id { get; set; }
        public int? Player3Id { get; set; }
        public int? Player4Id { get; set; }

        /// <summary>
        /// Was the game played on a desktop or cabinet
        /// </summary>
        public bool Desktop { get; set; }
        public byte BallsPerGame { get; set; }
        public string SystemVersion { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Ended { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Game Game { get; set; }
    }
}
