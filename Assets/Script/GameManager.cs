using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

public class GameManager : MonoBehaviour
{
    // ゲーム開始時に非表示にしたいUI（スタートボタンなど）を格納する
    public GameObject startUI;

    // ゲームが開始したかどうかを管理するフラグ
    private bool isGameStarted = false;

    void Update()
    {
        // ゲームが開始したら、ここから先の処理は行わない
        if (isGameStarted)
        {
            return;
        }

        // (ここにゲーム開始前の処理があれば記述)
    }

    // ボタンがクリックされた時に呼び出される公開メソッド
    public void StartGame()
    {
        if (isGameStarted) return; // 既に開始していたら何もしない

        isGameStarted = true;
        Debug.Log("ゲームスタート！");

        // スタートUIを非表示にする
        if (startUI != null)
        {
            startUI.SetActive(false);
        }

        // ここにゲーム開始時の処理を記述します
        // 例：プレイヤーの操作を有効にする、敵を出現させる など

        // もし別のシーンに遷移してゲームを始める場合
        // SceneManager.LoadScene("GameScene"); // "GameScene" はゲームプレイ用のシーン名
    }
}
