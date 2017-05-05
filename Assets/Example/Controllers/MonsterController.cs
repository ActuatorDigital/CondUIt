using MVC.Controllers;

namespace MVC.Example
{
    public class MonsterController : Controller
    {
        private IMonsterService _monsterService;

        protected override void GetServices(IServicesLoader services)
        {
            _monsterService = services.GetService<IMonsterService>();
        }

        public void Create ()
        {
            var monster = new MonsterModel();
            monster.Level = 1;
            View<CreateMonsterView>(monster);
        }

        public void Post (MonsterModel monster)
        {
            _monsterService.Create(monster);
            Action<TeamController>("MonsterList");
        }

        public void ConfirmDelete(MonsterModel monster)
        {
            View<DeleteMonsterView>(monster);
        }

        public void Delete(MonsterModel monster)
        {
            _monsterService.Delete(monster);
            Action<TeamController>("MonsterList");
        }
    }
}
