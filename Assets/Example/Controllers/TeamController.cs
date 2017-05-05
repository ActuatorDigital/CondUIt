using MVC.Controllers;

namespace MVC.Example
{
    public class TeamController : Controller
    {
        private ITeamServices _context;

        protected override void GetServices(IServicesLoader services)
        {
            _context = services.GetService<ITeamServices>();
        }

        public void MonsterList ()
        {
            View<TeamView>(_context.GetTeam());
        }
    }
}

