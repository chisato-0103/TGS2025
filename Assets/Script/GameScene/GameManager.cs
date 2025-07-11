using UnityEngine;
using TMPro; // TextMeshProを使用するために必要

public class GameManager : MonoBehaviour
{
    // --- スコア関連の変数 ---
    [SerializeField]
    private TextMeshProUGUI scoreText;
    //private int currentScore = 0;
    public static int currentScore;

    // --- タイマー関連の変数 ---
    [SerializeField]
    private TextMeshProUGUI timerText; // Inspectorからタイマー表示用UIテキストを設定

    [SerializeField]
    private float timeLimit = 60f; // ゲームの制限時間（秒）

    private float currentTime; // 残り時間を保持する変数
    private bool isGameActive; // ゲームがプレイ中かどうかを判定するフラグ

    public static GameManager Instance;  // ★追加：誰でもアクセスできる

    // ゲーム開始時に呼ばれる
    void Start()
    {
        // スコアの初期化
        currentScore = 0;
        UpdateScoreText();

        // --- タイマーの初期化 ---
        currentTime = timeLimit;
        isGameActive = true;
    }

    // 毎フレーム呼ばれる
    void Update()
    {
        // --- タイマーの更新処理 ---
        // ゲームがプレイ中でなければ、何もしない
        if (!isGameActive)
        {
            return;
        }

        // 時間を減らしていく
        currentTime -= Time.deltaTime;

        // もし残り時間が0以下になったら
        if (currentTime <= 0)
        {
            currentTime = 0; // マイナス表示を防ぐ
            isGameActive = false; // ゲームを非アクティブにする
            Debug.Log("ゲーム終了！");
            // ScreenManagerを探してResultSceneへ遷移
            ScreenManager screenManager = FindObjectOfType<ScreenManager>();
            if (screenManager != null)
            {
                screenManager.GoToResultScene();
            }
            else
            {
                Debug.LogError("ScreenManagerが見つかりませんでした");
            }
        }

        UpdateTimerText(); // 画面のタイマー表示を更新
    }


    // スコアを加算するためのメソッド
    public void AddScore(int points)
    {
        // ゲーム中でなければスコアは加算しない
        if (!isGameActive) return;

        currentScore += points;
        UpdateScoreText();
        Debug.Log("Score: " + currentScore);
    }

    // UIテキストを更新するためのメソッド
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    // --- タイマー表示を更新するメソッド ---
    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            // 残り時間を分と秒に変換
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // "00:00" の形式でテキストを表示
            timerText.text = string.Format("Time: {0:00}:{1:00}", minutes, seconds);
        }
    }

    public int getCurrentScore()
    {
        return currentScore;
    }
}
