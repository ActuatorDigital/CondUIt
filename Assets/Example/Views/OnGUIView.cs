using System;
using MVC.Controllers;
using MVC.Views;
using UnityEngine;

namespace MVC.Example
{
    /// <summary>
    /// Base class for a view that renders using Unity's GUILayout system.
    /// </summary>
    /// <typeparam name="M">Model Type</typeparam>
    /// <typeparam name="C">Controller Type</typeparam>
    public abstract class OnGUIView<M, C> : View<M, C>, IFadeable where C : IController
    {
        private float _alpha;
        private bool _isActive;

        public float Alpha
        {
            get
            {
                return _alpha;
            }

            set
            {
                _alpha = value;
            }
        }

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

            GUI.color = new Color(1f, 1f, 1f, _alpha);
            RenderElements();
            GUI.color = Color.white;
        }
    }
}

