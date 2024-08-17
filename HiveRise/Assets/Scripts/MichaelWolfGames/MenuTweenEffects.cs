using System;
using System.Collections;
using MichaelWolfGames;
using UnityEngine;

public static class MenuTweenEffects
{
    public static IEnumerator ScalePressEffect(RectTransform button, Action onClickAction, float scaleFactor = 1.25f, float duration = 1f, float delay = 0f, float finishDelay = 0.1f, bool useRealTime = false)
    {
        Vector3 startScale = button.localScale;
        Vector3 endScale = startScale * scaleFactor;
        yield return CoroutineExtensionMethods.CoDoTween(lerp =>
        {
            button.localScale = Vector3.LerpUnclamped(startScale, endScale, lerp);
        }, null, duration, delay, EaseType.punch, useRealTime);
        if (finishDelay > 0f)
        {
            if (useRealTime)
            {
                yield return new WaitForSecondsRealtime(finishDelay);
            }
            else
            {
                yield return new WaitForSeconds(finishDelay);
            }
        }
        if (onClickAction != null)
        {
            onClickAction();
        }
    }
    
    public static IEnumerator PopEmphasizeEffect(RectTransform rectTransform, Action onPeakAction, float scaleFactor = 1.25f, float duration = 1f, float delay = 0f, bool useRealTime = false)
    {
        Vector3 startScale = rectTransform.localScale;
        Vector3 endScale = startScale * scaleFactor;
        bool triggeredPeak = false;
        yield return CoroutineExtensionMethods.CoDoTween(lerp =>
        {
            rectTransform.localScale = Vector3.LerpUnclamped(startScale, endScale, Mathf.Sin(lerp * Mathf.PI));
            if (!triggeredPeak && lerp >= 0.5f)
            {
                triggeredPeak = true;
                onPeakAction?.Invoke();
            }
        }, null, duration, delay, EaseType.linear, useRealTime);
        
    }
    
}