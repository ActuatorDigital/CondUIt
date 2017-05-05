namespace MVC.Example
{
    public class TeamServices : ITeamServices
    {
        private TeamModel _team;

        public TeamServices(TeamModel team)
        {
            _team = team;
        }

        public TeamModel GetTeam()
        {
            return _team;
        }
    }
}
