namespace MVC.Example
{
    public class MonsterService : IMonsterService
    {
        private TeamModel _team;

        public MonsterService(TeamModel team)
        {
            _team = team;
        }

        public void Create(MonsterModel monster)
        {
            _team.Monsters.Add(monster);
        }

        public void Delete(MonsterModel monster)
        {
            _team.Monsters.Remove(monster);
        }
    }
}
