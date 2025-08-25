using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadGauge : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private float duration = 0f; // 必ず5秒で終わる

    private float elapsedTime = 0f;
    private bool finished = false;

    void Start()
    {
        progressBar.value = 0f;
        percentText.text = "0%";
    }

    void Update()
    {
        if (finished) return;

        // 経過時間
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        // すごく終盤で減速するカーブ
        float eased = EaseOutQuint(t);
        // もっと極端にしたい場合は EaseOutExpo(t) に変えてもOK

        // UIに反映
        progressBar.value = eased;
        int percent = Mathf.FloorToInt(eased * 100f);
        percentText.text = percent + "%";

        // 完了
        if (t >= 1f && !finished)
        {
            finished = true;
            Invoke("GoNextScene", 0.5f);
        }
    }

    // 終盤かなり減速するカーブ（Quint）
    float EaseOutQuint(float x)
    {
        return 1f - Mathf.Pow(1f - x, 2f);
    }

    // さらに極端にしたいならこれ（Expo）
    float EaseOutExpo(float x)
    {
        return (x == 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * x);
    }

    void GoNextScene()
    {
        screenManager.StartGame();
    }
}
