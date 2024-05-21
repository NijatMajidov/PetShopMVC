using PetShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShop.Business.Services.Abstract
{
    public interface ITeamService
    {
        void CreateTeam(Team team);
        void DeleteTeam(int id);
        void UpdateTeam(int id,Team team);
        Team GetTeam(Func<Team,bool>? func = null);
        List<Team> GetAllTeams(Func<Team, bool>? func = null);
    }
}
