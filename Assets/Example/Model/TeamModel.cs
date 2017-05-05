using MVC.Models;
using System.Collections.Generic;

namespace MVC.Example
{
    public class TeamModel : IModel
    {
        public List<MonsterModel> Monsters = new List<MonsterModel>();
    }
}
