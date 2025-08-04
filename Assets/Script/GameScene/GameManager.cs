using UnityEngine;
using TMPro; // TextMeshProを使用するために必要
using System.Collections;
using UnityEngine.Video; // VideoPlayerを使用するために必要

public class GameManager : MonoBehaviour
{
    // 現在のコンボ数
    private int currentCombo = 0;
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

     // 体力ゲージ（表面の常に見える部分）
    [SerializeField]
    private GameObject comboGauge;

    // 最大HP
    [SerializeField]
    private int maxCombo;
    // HP1あたりの幅
    private float combo1;

    // --- フィーバー動画関連の変数 ---
    [SerializeField]
    private VideoPlayer feverVideoPlayer; // VideoPlayerコンポーネントへの参照
    [SerializeField]
    private GameObject feverVideoUI; // 動画表示用のRawImageのGameObject

    private bool isFeverActive = false; // フィーバーモードが有効かどうか

    // --- BGM関連の変数 ---
    [SerializeField]
    private AudioSource bgmAudioSource; // BGM再生用のAudioSource
    // ゲーム開始時に呼ばれる
    void Start()
    {
        // スコアの初期化
        currentScore = 0;
        UpdateScoreText();

        // --- タイマーの初期化 ---
        currentTime = timeLimit;
        isGameActive = true;

        // ゲージの初期化（幅0に）
        if (comboGauge != null)
        {
            var rect = comboGauge.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, rect.sizeDelta.y);
        }

        // BGMの開始
        StartBGM();
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
            ScreenManager screenManager = FindFirstObjectByType<ScreenManager>();
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

    void Awake(){
        // コンボゲージの幅を最大コンボ数で割って1コンボあたりの幅をcombo1に入れておく
        combo1 = comboGauge.GetComponent<RectTransform>().sizeDelta.x / maxCombo;
        Debug.Log("combo1初期化: " + combo1 + " maxCombo: " + maxCombo + " ゲージ初期幅: " + comboGauge.GetComponent<RectTransform>().sizeDelta.x);
        
        // VideoPlayerの設定を初期化時に行う
        if (feverVideoPlayer != null)
        {
            feverVideoPlayer.Prepare();
        }
    }

    // コンボ成立時にゲージを増やす
    public void AddCombo(int combo){
        // combo値分ゲージを増やす
        currentCombo += combo;
        float addWidth = combo1 * combo; // 追加するコンボ分だけの幅を計算
        StartCoroutine(IncreaseComboGauge(addWidth));
        Debug.Log("Combo: " + currentCombo);
    }

    // ポイントにならないターゲット消去時の処理
    public void ResetComboAndHalveGauge()
    {
        // 現在のゲージ幅取得
        Vector2 nowSize = comboGauge.GetComponent<RectTransform>().sizeDelta;
        // ゲージ幅を半分に
        nowSize.x *= 0.5f;
        comboGauge.GetComponent<RectTransform>().sizeDelta = nowSize;
        
        // ゲージ幅に対応するコンボ数を計算してcurrentComboに設定
        currentCombo = Mathf.RoundToInt(nowSize.x / combo1);
        Debug.Log("Combo Reset: " + currentCombo);
    }

    // コンボゲージを増やすコルーチン
    IEnumerator IncreaseComboGauge(float addWidth){
        // 現在のゲージ幅取得
        Vector2 nowSize = comboGauge.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("ゲージ増加前: " + nowSize.x + " 追加幅: " + addWidth);
        
        // ゲージの幅を加算
        nowSize.x += addWidth;
        // 最大幅を超えないように制限
        float maxWidth = combo1 * maxCombo;
        bool wasMaxBefore = nowSize.x - addWidth >= maxWidth; // 加算前に既に最大だったか
        Debug.Log("最大幅: " + maxWidth + " 現在幅: " + nowSize.x + " 加算前に最大だった: " + wasMaxBefore);
        
        if (nowSize.x > maxWidth) nowSize.x = maxWidth;
        // ゲージに反映
        comboGauge.GetComponent<RectTransform>().sizeDelta = nowSize;

        // コンボゲージが最大に達した場合の処理
        if (!wasMaxBefore && nowSize.x >= maxWidth && !isFeverActive)
        {
            Debug.Log("フィーバーモード発動条件を満たしました");
            StartFeverMode();
        }

        yield return null;
    }

    // フィーバーモード開始（動画再生）
    private void StartFeverMode()
    {
        if (feverVideoPlayer == null || feverVideoUI == null)
        {
            Debug.LogWarning("VideoPlayerまたはVideoUIが設定されていません");
            return;
        }

        isFeverActive = true;
        Debug.Log("フィーバーモード開始！");

        // 動画UIを表示
        feverVideoUI.SetActive(true);

        // 動画再生開始
        feverVideoPlayer.Play();

        // 動画終了時のコールバックを設定
        feverVideoPlayer.loopPointReached += OnFeverVideoEnd;
    }

    // 動画終了時の処理
    private void OnFeverVideoEnd(VideoPlayer vp)
    {
        Debug.Log("フィーバー動画終了");

        // 動画UIを非表示
        feverVideoUI.SetActive(false);

        // コールバックを解除
        feverVideoPlayer.loopPointReached -= OnFeverVideoEnd;

        // フィーバーモード終了
        isFeverActive = false;
    }

    // フィーバーモードを手動で停止する場合のメソッド
    public void StopFeverMode()
    {
        if (feverVideoPlayer != null && feverVideoPlayer.isPlaying)
        {
            feverVideoPlayer.Stop();
            OnFeverVideoEnd(feverVideoPlayer);
        }
    }


    // --- BGM制御メソッド ---
    private void StartBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.clip != null)
        {
            bgmAudioSource.Play();
            Debug.Log("BGM開始: " + bgmAudioSource.clip.name);
        }
        else
        {
            Debug.LogWarning("BGM AudioSourceまたはAudioClipが設定されていません");
        }
    }

    public void StopBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
            Debug.Log("BGM停止");
        }
    }

    public void SetBGMVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = Mathf.Clamp01(volume);
        }
    }
}
