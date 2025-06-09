using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

public class TitleScreenManager: MonoBehaviour
{
    public void Update()
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
}
