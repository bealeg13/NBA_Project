using Newtonsoft.Json.Linq;

namespace NbaProject.Models
{
    public class TeamApiCall
    {
        public static List<TeamRoster> GetAllNbaTeams()
        {
            var client = new HttpClient();
            var teamURL = "https://www.balldontlie.io/api/v1/teams";

            try
            {
                var response = client.GetStringAsync(teamURL).Result;

                JObject formattedResponse = JObject.Parse(response);
                List<TeamRoster> teams = new List<TeamRoster>();

                for (int i = 0; i <= 29; i++)
                {
                    string teamName = formattedResponse["data"][i]["full_name"].ToString();

                    TeamRoster team = new TeamRoster
                    {
                        TeamName = teamName
                    };

                    teams.Add(team);
                }

                return teams;
            }
            catch (Exception ex)
            {
                // Handle the exception as needed.
                // You might want to log it or return an error message.
            }

            return new List<TeamRoster>();
        }
    }
}
