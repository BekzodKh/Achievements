using System.Threading;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using UnityEngine;

namespace Core.Utils.Extensions
{
	public static class TweenExtensions
	{
		public static UniTask AwaitForCompleteWithKilling(this Tween tween, CancellationToken cancellationToken)
		{
			CancellationTokenRegistration registration = new CancellationTokenRegistration();
			
			registration = cancellationToken.Register(() =>
			{
				if (tween.IsActive())
				{
					tween.Kill();
				}
				
				registration.Dispose();
			});
			
			tween.OnComplete(() =>
			{
				registration.Dispose();
			});
			
			return tween.AwaitForComplete(cancellationToken: cancellationToken);
		}
		
		public static Tween DORotateX(this Transform target, float xValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			return target.DORotate(new Vector3(xValue, target.rotation.eulerAngles.y, target.rotation.eulerAngles.z),
				duration, mode);
		}
		
		public static Tween DORotateY(this Transform target, float yValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			return target.DORotate(new Vector3(target.rotation.eulerAngles.x, yValue, target.rotation.eulerAngles.z),
				duration, mode);
		}
		
		public static Tween DORotateZ(this Transform target, float zValue, float duration, RotateMode mode = RotateMode.Fast)
		{
			return target.DORotate(new Vector3(target.rotation.eulerAngles.x, target.rotation.eulerAngles.y, zValue),
				duration, mode);
		}
	}
}