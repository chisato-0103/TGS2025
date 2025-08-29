using UnityEngine;

public class RankingSceneManager_senser : MonoBehaviour
{
    // M5StickReaderへの参照を保持する変数
    private M5StickReader m5StickReader;

    void Start()
    {
        // シーン内からM5StickReaderコンポーネントを探してくる
        m5StickReader = M5StickReader.Instance;
    }

    void Update()
    {
        // 遷移可能な状態でマウスクリックまたはタップを検知
        if (m5StickReader.ConsumeThrowActionFlag() && !m5StickReader.getThrowedActionFlag())
        {
            Debug.Log("Gコンが投げる動作をしました - ストーリーシーンへ遷移");
            ScreenManager screenManager = FindObjectOfType<ScreenManager>();
            if (screenManager != null)
            {
                screenManager.GoToTitleScene();
            }
            else
            {
                Debug.LogError("ScreenManagerが見つかりませんでした");
            }
        }
    }
}
