using UnityEngine;

public class TitleSceneManager_senser : MonoBehaviour
{
    [SerializeField]
    private float inputDelay = 10.0f; // 入力受付までの遅延時間

    private bool canTransition = false; // 遷移可能フラグ

    // M5StickReaderへの参照を保持する変数
    private M5StickReader m5StickReader;

    void Start()
    {
        // シーン内からM5StickReaderコンポーネントを探してくる
        m5StickReader = M5StickReader.Instance;
        // 最初は遷移不可
        canTransition = false;
        Debug.Log("タイトルシーン開始 - 入力受付まで" + inputDelay + "秒待機");

        // 遅延後に入力受付開始
        Invoke("EnableTransition", inputDelay);
    }

    void Update()
    {
        
        // 遷移可能な状態でマウスクリックまたはタップを検知
        if (m5StickReader.getButtonFlag())
        {
            m5StickReader.setPushedButton(true);
        }

        
        if (canTransition && m5StickReader.Consumepushedbutton() && !m5StickReader.getButtonFlag())
        {
            Debug.Log("Gコンが投げる動作をしました - ストーリーシーンへ遷移");
            ScreenManager screenManager = FindObjectOfType<ScreenManager>();
            m5StickReader.setPushedButton(false);
            if (m5StickReader.getTarget_y() > -3.0f)
            {
                if(screenManager != null)
                    screenManager.GoToStoryScene();
                else
                    Debug.LogError("ScreenManagerが見つかりませんでした");
            }
        }

        /*
        if (canTransition && m5StickReader.ConsumeThrowActionFlag() && !m5StickReader.getThrowedActionFlag())
        {
            Debug.Log("Gコンが投げる動作をしました - ストーリーシーンへ遷移");
            ScreenManager screenManager = FindObjectOfType<ScreenManager>();
            if (screenManager != null)
            {
                screenManager.GoToStoryScene();
            }
            else
            {
                Debug.LogError("ScreenManagerが見つかりませんでした");
            }
        }
        */
    }

    private void EnableTransition()
    {
        canTransition = true;
        Debug.Log("タイトル画面 - センサー入力受付開始");
    }
}