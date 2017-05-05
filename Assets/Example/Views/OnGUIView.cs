using MVC.Controllers;
using MVC.Views;
using UnityEngine;

namespace MVC.Example
{
    public abstract class OnGUIView<M, C> : View<M, C> where C : IController
    {
        private bool _isActive;

        protected override void ClearElements()
        {
            _isActive = false;
        }

        protected override void LoadElements()
        {
            _isActive = true;
        }

        protected abstract void RenderElements();

        private void OnGUI()
        {
            if (!_isActive)
                return;

            RenderElements();
        }
    }
}

