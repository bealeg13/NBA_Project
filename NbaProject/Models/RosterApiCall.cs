using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace NbaProject.Models
{
    public class RosterApiCall
    {
        public static async Task<List<TeamRoster>> GetTeamRostersAsync()
        {
            {
                var client = new HttpClient();
                var season = "2022";
                int page = 1; // Start with page 1
                int playersPerPage = 25;
                int totalPlayersToRetrieve = 498; // Set the total number of players you want to retrieve

                //List<string> rosterInformation = new List<string>();

                try
                {
                    int totalPlayers = 0; // Initialize the total players count

                    Dictionary<string, PlayerInfo> playerInfoDictionary = new Dictionary<string, PlayerInfo>();
                    Dictionary<string, List<PlayerInfo>> teamRosters = new Dictionary<string, List<PlayerInfo>>();

                    while (totalPlayers < totalPlayersToRetrieve)
                    {
                        var playerURL = $"https://www.balldontlie.io/api/v1/stats?seasons[]={season}&page={page}";

                        var response = client.GetStringAsync(playerURL).Result;
                        JObject formattedResponse = JObject.Parse(response);
                        var playerData = formattedResponse["data"];

                        if (playerData != null)
                        {
                            foreach (var player in playerData)
                            {

                                var playerFirstName = player["player"]["first_name"];
                                var playerLastName = player["player"]["last_name"];
                                var playerFullName = playerFirstName + " " + playerLastName;
                                var playerPosition = (string)player["player"]["position"];
                                var playerTeamName = (string)player["team"]["full_name"];


                                //Console.WriteLine($"Name: {playerFullName}  Position: {playerPosition}  Team: {playerTeamName}");
                                var playerInfo = new PlayerInfo
                                {
                                    PlayerName = playerFullName,
                                    TeamName = playerTeamName,
                                    Position = new Dictionary<string, string>
                            {
                                { "Position", playerPosition },

                            }
                                };

                                // Add the player information to the dictionary
                                //playerInfoDictionary[playerFullName] = playerInfo;


                                if (!teamRosters.ContainsKey(playerTeamName))
                                {
                                    teamRosters[playerTeamName] = new List<PlayerInfo>();
                                }
                                teamRosters[playerTeamName].Add(playerInfo);

                                totalPlayers++;
                            }

                            // Move to the next page
                            page++;
                        }
                        else
                        {
                            // No more data available, break the loop
                            break;
                        }
                    }

                    List<TeamRoster> teamRosterlist = new List<TeamRoster>();
                    foreach (var teamRosterEntry in teamRosters)
                    {
                        TeamRoster teamRoster = new TeamRoster
                        {
                            TeamName = teamRosterEntry.Key,
                            Roster = teamRosterEntry.Value
                        };
                        teamRosterlist.Add(teamRoster);

                    }
                    return teamRosterlist;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<TeamRoster>();
                }

                //return new List<TeamRoster>();
            }
        }

        public static async Task<List<PlayerInfo>> GetSingleRosterAsync(string teamName)
        {
            var teamRosters = await GetTeamRostersAsync();

            var roster = teamRosters.FirstOrDefault(roster => roster.TeamName == teamName);

            if (roster != null)
            {
                return roster.Roster;
            }
            else
            {
                // Return an empty list if the team is not found
                return new List<PlayerInfo>();
            }
        }



        //public static async Task<List<PlayerInfo>> GetSingleTeamRosterAsync(string teamName)
        //{
        //    var client = new HttpClient();
        //    var season = "2022";
        //    int page = 1;
        //    int playersPerPage = 25;

        //    List<PlayerInfo> teamRoster = new List<PlayerInfo>();

        //    try
        //    {
        //        while (true)
        //        {
        //            var playerURL = $"https://www.balldontlie.io/api/v1/stats?seasons[]={season}&page={page}";

        //            var response = await client.GetStringAsync(playerURL);
        //            JObject formattedResponse = JObject.Parse(response);
        //            var playerData = formattedResponse["data"];

        //            if (playerData != null)
        //            {
        //                foreach (var player in playerData)
        //                {
        //                    var playerFirstName = player["player"]["first_name"];
        //                    var playerLastName = player["player"]["last_name"];
        //                    var playerFullName = playerFirstName + " " + playerLastName;
        //                    var playerPosition = (string)player["player"]["position"];
        //                    var playerTeamName = (string)player["team"]["full_name"];

        //                    if (playerTeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        var playerInfo = new PlayerInfo
        //                        {
        //                            PlayerName = playerFullName,
        //                            TeamName = playerTeamName,
        //                            Position = new Dictionary<string, string>
        //                    {
        //                        { "Position", playerPosition },
        //                    }
        //                        };

        //                        teamRoster.Add(playerInfo);
        //                    }
        //                }

        //                // Move to the next page
        //                page++;
        //            }
        //            else
        //            {
        //                // No more data available, break the loop
        //                break;
        //            }
        //        }

        //        return teamRoster;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new List<PlayerInfo>();
        //    }
        //}

    }

}
