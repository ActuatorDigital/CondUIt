using UnityEngine;

namespace MVC.Example
{
    public class DeleteMonsterView : OnGUIView<MonsterModel, MonsterController>
    {
        private const string TITLE = "Are you sure you wish to delete {0}?";

        protected override void RenderElements()
        {
            GUILayout.Label(string.Format(TITLE, Model.Name));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel"))
                GoBackToTeamList();
            if (GUILayout.Button("Delete"))
                ConfirmDelete();
            GUILayout.EndHorizontal();
        }

        private void ConfirmDelete()
        {
            Action("Delete", Model);
            Hide();
        }

        private void GoBackToTeamList()
        {
            Action<TeamController>("MonsterList");
            Hide();
        }
    }
}

