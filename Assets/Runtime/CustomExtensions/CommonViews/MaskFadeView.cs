using DG.Tweening;

using UnityEngine;

namespace Core.Runtime.CommonViews
{
	public class MaskFadeView : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer _mask;
    
		[SerializeField, Space, Range(0f, 1f)]
		private float _fadeInPercent;

		[SerializeField, Range(0f, 1f)]
		private float _fadeOutPercent;
    
		[SerializeField, Space]
		private float _fadeInDuration = 0.1f;
    
		[SerializeField]
		private float _fadeOutDuration = 0.1f;

		private Tween _currentTween;

		private void OnValidate()
		{
			if (_mask == null)
			{
				_mask = GetComponent<SpriteRenderer>();
			}
		}

		public void FadeIn()
		{
			Fade(_fadeInPercent, _fadeInDuration);
		}

		public void FadeOut()
		{
			Fade(_fadeOutPercent, _fadeOutDuration);
		}

		private void Fade(float fadePercent, float fadeTime)
		{
			if (_currentTween.IsActive())
			{
				_currentTween.Kill();
			}
        
			_currentTween = _mask.DOFade(fadePercent, fadeTime);
		}
	}
}