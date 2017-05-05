using UnityEngine;

namespace MVC.Example
{
    public class TeamView : OnGUIView<TeamModel, TeamController>
    {
        private const string TEAM_TITLE = "Current Team ({0} / 6)";
        private const string INDEX_TITLE = "#{0}: ";
        private const string LVL_TEXT = "Lv {0}";
        private const string ADD_BUTTON_TEXT = "Add";

        protected override void RenderElements()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            GUILayout.Label(string.Format(TEAM_TITLE, Model.Monsters.Count));

            if(Model.Monsters.Count < 6)
            {
                if (GUILayout.Button(ADD_BUTTON_TEXT))
                    AddNewMonster();
            }

            GUILayout.EndHorizontal();

            for (int i = 0; i < Model.Monsters.Count; i++)
                RenderMonster(Model.Monsters[i], i + 1);

            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }

        private void RenderMonster (MonsterModel monster, int i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(string.Format(INDEX_TITLE, i));
            GUILayout.Label(monster.Name);
            GUILayout.FlexibleSpace();
            GUILayout.Label(string.Format(LVL_TEXT, monster.Level));
            if (GUILayout.Button("X"))
                DeleteMonster(monster);
            GUILayout.EndHorizontal();
        }

        private void AddNewMonster ()
        {
            Action<MonsterController>("Create");
            Hide();
        }

        private void DeleteMonster(MonsterModel monster)
        {
            Action<MonsterController>("ConfirmDelete", monster);
            Hide();
        }
    }
}

