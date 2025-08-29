using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private float inputDelay = 1.0f; // 入力受付までの遅延時間
    
    private bool canTransition = false; // 遷移可能フラグ
    
    void Start()
    {
        // 最初は遷移不可
        canTransition = false;
        Debug.Log("タイトルシーン開始 - 入力受付まで" + inputDelay + "秒待機");
        
        // 遅延後に入力受付開始
        Invoke("EnableTransition", inputDelay);
    }
    
    void Update()
    {
        // 遷移可能な状態でマウスクリックまたはタップを検知
        if (canTransition && Input.GetMouseButtonDown(0))
        {
            Debug.Log("画面がクリックされました - ストーリーシーンへ遷移");
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
    }
    
    private void EnableTransition()
    {
        canTransition = true;
        Debug.Log("タイトル画面 - 入力受付開始");
    }
}