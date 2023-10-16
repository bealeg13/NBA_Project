using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NbaProject.Models;

namespace NbaProject.Controllers
{
    public class TeamController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var teams = TeamApiCall.GetAllNbaTeams();
            return View(teams);
        }

        public async Task<IActionResult> ViewRoster(string teamName)
        {
            var roster = await RosterApiCall.GetSingleRosterAsync(teamName);
            if (roster == null)
            {
                return NotFound(); 
            }

            var teamRoster = new NbaProject.TeamRoster
            {
                TeamName = teamName,
                Roster = roster
            };

            return View(teamRoster);





        }
    }

    
    
}
