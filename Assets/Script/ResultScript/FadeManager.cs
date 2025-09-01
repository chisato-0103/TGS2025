using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    void Start()
    {
        // 最初はフェードインせず、透明にしておく
        Color c = fadeImage.color;
        c.a = 0f;  // 透明
        fadeImage.color = c;
    }

    public IEnumerator FadeOut(Action onComplete)
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        onComplete?.Invoke();
    }

    public IEnumerator FadeIn(Action onComplete)
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }
        onComplete?.Invoke();
    }
}
