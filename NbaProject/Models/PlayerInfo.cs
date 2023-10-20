namespace NbaProject.Models
{
    public class PlayerInfo
    {
        public string PlayerName { get; set; }
        public string TeamName { get; set; }
        public Dictionary<string, string> Position { get; set; }
    }
}
