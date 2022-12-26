namespace FF.Shared.ViewModel.Scores
{
    public class ScoreResultUserMachineVm
    {
        public string Points { get; set; }
        public string Initials { get; set; }
        public TimeSpan GameTime { get; set; }
        public DateTime Ended { get; set; }
        public int Balls { get; set; }
        public string Title { get; set; }
        public string CRC { get; set; }
        public string SimVersion { get; set; }
        public bool Desktop { get; set; }
        public string MachineName { get; set; }
        public string MachineUser { get; set; }
    }
}