using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Fade : MonoBehaviour
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
        while (t < fadeDuration)//フェード処理の条件
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;//これをつけることにより1フレームごとに暗くすることを可能にしている
        }
        onComplete?.Invoke();//これでフェード後の処理を行っている
    }

    public IEnumerator FadeIn(Action onComplete)
    {
        float t = 0;
        Color c = fadeImage.color;
        while (t < fadeDuration)//フェードの条件
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = c;
            yield return null;//１フレームごとにフェードを動かすための処理
        }
        onComplete?.Invoke();//フェード後の処理を行う
    }
}
