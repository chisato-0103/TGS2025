using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadGauge : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private float duration = 0f; // 必ず◯秒で終わる

    private float elapsedTime = 0f;
    private bool finished = false;
    private Fade fade;

    void Start()
    {
        progressBar.value = 0f;
        percentText.text = "0%";
        fade = FindObjectOfType<Fade>(); // フェード管理クラスを探す
    }

    void Update()
    {
        if (finished) return;

        // 経過時間
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        // すごく終盤で減速するカーブ
        float eased = EaseOutQuint(t);

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

    void GoNextScene()
    {
        // 4秒経ったらフェードアウト開始
        StartCoroutine(fade.FadeOut(() => //() => {...}のような処理をラムダ式と言い()以前の処理を行なった後にやる処理を書くときに使う。またこれを使わなかった場合はしっかりフェードしないで次の処理に行く可能性があるのでこうしている。
        {
            // フェードアウト終了後にシーン切り替え
            screenManager.GoToTutorialScene();

            // 次のシーンに移ったらフェードイン開始
            StartCoroutine(fade.FadeIn(() => { /* 何もしない */ }));
        }));
    }
}
