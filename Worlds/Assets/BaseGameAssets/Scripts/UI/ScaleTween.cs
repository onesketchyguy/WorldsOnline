using UnityEngine;

namespace Worlds.UI
{
    public class ScaleTween : MonoBehaviour
    {
        public LeanTweenType easeType;
        public AnimationCurve curve;

        public float duration = 0.5f;

        public float delay;

        public Vector3 scale;

        private void OnValidate()
        {
            scale = transform.localScale;
        }

        private void OnEnable()
        {
            if (scale != Vector3.zero)
            {
                LeanTween.scale(gameObject, Vector3.zero, 0);
            }

            if (easeType == LeanTweenType.animationCurve)
                LeanTween.scale(gameObject, scale, duration).setDelay(delay).setEase(curve);
            else LeanTween.scale(gameObject, scale, duration).setDelay(delay).setEase(easeType);
        }
    }
}