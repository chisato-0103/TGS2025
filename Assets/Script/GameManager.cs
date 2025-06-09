using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("ゲームスタート！");

        SceneManager.LoadScene("GameScene"); // "GameScene" はゲームプレイ用のシーン名
    }
}
