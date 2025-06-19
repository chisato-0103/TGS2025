// GameManager.cs

using UnityEngine;
using TMPro; // TextMeshProを使用するために必要

public class GameManager : MonoBehaviour
{
    // Inspectorからスコア表示用のUIテキストを設定する
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // 現在のスコアを保持する変数
    private int currentScore = 0;

    // ゲーム開始時に呼ばれる
    void Start()
    {
        // 最初はスコア0で表示を初期化
        currentScore = 0;
        UpdateScoreText();
    }

    // スコアを加算するためのメソッド（他のスクリプトから呼び出す）
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreText();
        Debug.Log("Score: " + currentScore); // 念のためコンソールにも出力
    }

    // UIテキストを更新するためのメソッド
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}
