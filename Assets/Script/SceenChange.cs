using UnityEngine;
using UnityEngine.SceneManagement;

public class SceenChange : MonoBehaviour
{
    private float clickEnableDelay = 0.2f; // 0.2秒後から有効に
    private float timer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > clickEnableDelay) //前のシーンで画面クリックによりシーン変更を利用したためそのままだとランキングシーンからゲームシーンに飛んでしまうためそれをなくすため画面クリック処理に遅延をかけた
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("画面がクリックされました");
                ScreenManager screenManager = FindObjectOfType<ScreenManager>();
                if (screenManager != null)
                {
                    Debug.Log("ゲームスタート！");
                    screenManager.GoToLoadScene();
                }
                else
                {
                    Debug.LogError("ScreenManagerが見つかりませんでした");
                }
            }
        }
    }
}
