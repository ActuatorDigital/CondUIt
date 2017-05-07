namespace MVC.Views
{
    public class OnOffTransition : TransitionHandler
    {
        public override void OnHide()
        {
            SetActive(false);
        }

        public override void OnShow()
        {
            SetActive(true);
        }

        private void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}

