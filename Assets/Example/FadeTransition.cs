using System.Collections;
using UnityEngine;
using System;

namespace MVC.Views
{
    public class FadeTransition : TransitionHandler
    {
        [SerializeField]
        private float _fadeDuration;
        [SerializeField]
        private Component _element;

        private IFadeable _fadeable;
        private IEnumerator _fadeAnimation;

        public void Awake()
        {
            _fadeable = _element as IFadeable;
        }

        public override void OnHide()
        {
            PlayFade(0f);
        }

        public override void OnShow()
        {
            PlayFade(1f);
        }

        private void PlayFade (float target)
        {
            if (_fadeable == null)
                return;

            if (_fadeAnimation != null)
                StopCoroutine(_fadeAnimation);

            _fadeAnimation = Fade(target);
            StartCoroutine(_fadeAnimation);
        }

        private IEnumerator Fade (float target)
        {
            var currentAlpha = _fadeable.Alpha;
            var timer = 0f;

            while(timer < 1f)
            {
                timer += Time.deltaTime / _fadeDuration;
                _fadeable.Alpha = Mathf.Lerp(currentAlpha, target, timer);
                yield return null;
            }

            _fadeAnimation = null;
        }
    }
}

