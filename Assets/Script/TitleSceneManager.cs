using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button titleButton; // タイトルシーンの画面全体ボタン
    
    [SerializeField]
    private float inputDelay = 0.5f; // 入力受付までの遅延時間
    
    void Start()
    {
        // 最初はボタンを無効化
        if (titleButton != null)
        {
            titleButton.interactable = false;
            Debug.Log("タイトルボタン一時無効化");
            
            // 遅延後にボタンを有効化
            Invoke("EnableTitleButton", inputDelay);
        }
    }
    
    private void EnableTitleButton()
    {
        if (titleButton != null)
        {
            titleButton.interactable = true;
            Debug.Log("タイトルボタン有効化");
        }
    }
}