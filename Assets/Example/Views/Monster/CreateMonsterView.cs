using UnityEngine;

namespace MVC.Example
{
    public class CreateMonsterView : OnGUIView<MonsterModel, MonsterController>
    {
        private const string TITLE = "New Monster";
        private const string NAME_TITLE = "Name: ";
        private const string ENTER_NAME_TEXT = "Please enter a name for your monster.";
        private const string SUBMIT_TEXT = "Create";

        private string _name = "";

        protected override void ClearElements()
        {
            base.ClearElements();
            _name = "";
        }

        protected override void RenderElements()
        {
            GUILayout.Label(TITLE);
            GUILayout.BeginHorizontal();

            GUILayout.Label(NAME_TITLE);
            _name = GUILayout.TextField(_name, GUILayout.Width(200));

            GUILayout.EndHorizontal();

            if (string.IsNullOrEmpty(_name))
                GUILayout.Label(ENTER_NAME_TEXT);
            else
                if (GUILayout.Button(SUBMIT_TEXT))
                    Submit();
        }

        private void Submit ()
        {
            Model.Name = _name;
            Action("Post", Model);
            Hide();
        }
    }
}

