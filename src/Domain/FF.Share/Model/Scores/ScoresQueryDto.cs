namespace FF.Shared.Model.Scores
{
    public class ScoresQueryDto : PageQueryBaseDto
    {
        public string UserId { get; set; }
        /// <summary>
        /// limit results to the userId
        /// </summary>
        public bool GetUsersScores { get; set; }
        public string UserName { get; set; }
        public string GameId { get; set; }
        public string Title { get; set; }
        public long PlayerId { get; set; }
        public ScoresOrderBy OrderBy{ get; set; }
        public SortDirection SortOrder { get; set; }
    }
}
