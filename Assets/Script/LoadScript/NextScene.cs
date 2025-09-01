using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private float start = 0;
    [SerializeField] private float end = 4; // 待機秒数
    private bool flag = false;

    private ScreenManager screenManager;
    private Fade fade;

    void Start()
    {
        screenManager = FindObjectOfType<ScreenManager>();
        fade = FindObjectOfType<Fade>(); // フェード管理クラスを探す
    }

    void Update()
    {
        if (!flag)
        {
            start += Time.deltaTime;

            if (start >= end)
            {
                flag = true;
                // フェードアウト開始
                StartCoroutine(fade.FadeOut(() =>
                {
                    // フェードアウト終了後にシーン切り替え
                    screenManager.StartGame();

                }));
            }
        }
    }
}
