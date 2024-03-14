using System;
using System.Threading;

using UnityEngine;

using Cysharp.Threading.Tasks;

using DG.Tweening;

namespace Core.Runtime.CommonViews
{
    public class CircleMaskView : MonoBehaviour
    {
        [SerializeField] private Transform _maskTransform;
        [SerializeField] private GameObject[] _maskObjects;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Vector2 _targetScale = new Vector2(8, 8);
        [SerializeField] private float _showDuration = 1f;
        [SerializeField] private float _hideDuration = 0.8f;
        [SerializeField] private float _maskOnDelay = 0.25f;
        [SerializeField] private float _showSoundDelay = 0.3f;
        [SerializeField] private float _hideMaskDelay = 0.1f;
        [SerializeField] private AudioClip _showSound;
        [SerializeField] private AudioClip _hideSound;
        [SerializeField] private AudioSource _audioSource;

        private Tween _tween;

        public async UniTask ShowAsync(CancellationToken token = default, bool playSound = true)
        {
            SetActiveState(true);

            if (playSound)
            {
                UniTask.Delay(TimeSpan.FromSeconds(_showSoundDelay), cancellationToken: token)
                    .ContinueWith(() => _audioSource.PlayOneShot(_showSound)).Forget();
            }
            
            token.Register(ResetToDefault);
            
            await PlayDoScaleAsync(Vector2.zero, _showDuration, token);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_maskOnDelay), cancellationToken: token);
        }

        public async UniTask HideAsync(CancellationToken token = default, bool playSound = true)
        {
            if (playSound)
            {
                _audioSource.PlayOneShot(_hideSound);
            }
            
            token.Register(ResetToDefault);
            
            await UniTask.Delay(TimeSpan.FromSeconds(_hideMaskDelay), cancellationToken: token)
                .ContinueWith(() => PlayDoScaleAsync(_targetScale, _hideDuration, token)); 

            SetActiveState(false);
        }
        
        public void ResetToDefault()
        {
            ClearTween();
            _maskTransform.localScale = _targetScale;
            SetActiveState(false);
        }
        
        private async UniTask PlayDoScaleAsync(Vector3 scaleTo, float duration, CancellationToken token = default)
        {
            ClearTween();
            _tween = _maskTransform.DOScale(scaleTo, duration).SetEase(Ease.InOutSine);
            await _tween.AwaitForComplete(cancellationToken: token);
        }
        
        private void SetActiveState(bool state)
        {
            _collider.enabled = state;

            foreach (var maskObject in _maskObjects)
            {
                maskObject.SetActive(state);
            }
        }

        private void ClearTween()
        {
            if (_tween.IsActive())
            {
                _tween.Kill();
            }
        }
    }
}