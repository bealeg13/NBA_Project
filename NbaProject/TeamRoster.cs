using NbaProject.Models;

namespace NbaProject
{
    public class TeamRoster
    {
        public string TeamName { get; set; }
        public List<PlayerInfo> Roster { get; set; }
    }
}