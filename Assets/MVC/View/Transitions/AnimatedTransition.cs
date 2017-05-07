using System.Collections;
using UnityEngine;

namespace MVC.Views
{
    [RequireComponent(typeof(Animator))]
    public class AnimatedTransition : TransitionHandler
    {
        [SerializeField]
        private Animator _animator = null;

        public override void OnHide()
        {
            PlayTransition(false);
        }

        public override void OnShow()
        {
            gameObject.SetActive(true);
            StartCoroutine(PlayTransitionAtEndOfFrame(true));
        }

        private void PlayTransition(bool isVisible)
        {
            _animator.SetBool("IsVisible", isVisible);
        }

        private IEnumerator PlayTransitionAtEndOfFrame(bool isShowing)
        {
            yield return new WaitForEndOfFrame();
            PlayTransition(isShowing);

        }
    }
}

