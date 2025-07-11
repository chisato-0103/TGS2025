using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

public class ScreenManager: MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }
    public void StartGame()
    {
        Debug.Log("ゲームスタート！");

        SceneManager.LoadScene("GameScene"); // "GameScene" はゲームプレイ用のシーン名
    }

    // ゲーム終了時にResultSceneへ遷移するメソッド
    public void GoToResultScene()
    {
        Debug.Log("リザルト画面へ遷移");
        SceneManager.LoadScene("ResultScene"); // "ResultScene" はリザルト用のシーン名
    }
}
