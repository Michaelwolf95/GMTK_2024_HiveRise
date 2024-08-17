using System;
using System.Collections;
using UnityEngine;

namespace MichaelWolfGames
{
    public static class TweenExtensions
    {
        public static Coroutine InvokeMethod(this MonoBehaviour monoBehaviour, Action onComplete, float duration)
        {
            return monoBehaviour.StartCoroutine(CoInvoke(monoBehaviour, onComplete, duration));

        }

        private static IEnumerator CoInvoke(this MonoBehaviour monoBehaviour, Action onComplete, float duration)
        {
            yield return new WaitForSeconds(duration);
            if (onComplete != null)
            {
                onComplete();
            }
        }
        
        
        public static Coroutine LerpTween(this MonoBehaviour monoBehaviour, Action<float> onUpdate, Action onComplete, float duration)
        {
            return monoBehaviour.StartCoroutine(CoLerpTween(monoBehaviour, onUpdate, onComplete, duration));

        }

        private static IEnumerator CoLerpTween(this MonoBehaviour monoBehaviour, Action<float> onUpdate, Action onComplete, float duration)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                onUpdate(timer / duration);
                yield return null;
            }
            if (onComplete != null)
            {
                onComplete();
            }
        }
        
    }
}