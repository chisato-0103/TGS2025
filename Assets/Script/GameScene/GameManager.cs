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
    private bool isTimerPaused = false; // タイマーが一時停止中かどうかを判定するフラグ

    public static GameManager Instance;  // ★追加：誰でもアクセスできる

     // 体力ゲージ（表面の常に見える部分）
    [SerializeField]
    private GameObject comboGauge;

    // 最大HP
    [SerializeField]
    private int maxCombo;
    // HP1あたりの幅
    private float combo1;

    // --- 透過動画関連の変数 ---
    [SerializeField]
    private VideoPlayer preEffectVideoPlayer; // 透過動画用VideoPlayerコンポーネント
    [SerializeField]
    private GameObject preEffectVideoUI; // 透過動画表示用のRawImageのGameObject

    // --- フィーバー動画関連の変数 ---
    [SerializeField]
    private VideoPlayer feverVideoPlayer; // VideoPlayerコンポーネントへの参照
    [SerializeField]
    private GameObject feverVideoUI; // 動画表示用のRawImageのGameObject

    private bool isFeverActive = false; // フィーバーモードが有効かどうか


    // --- フィーバータイム関連の変数 ---
    [SerializeField]
    private float comboGaugeDecreaseRate = 2f; // フィーバー中のゲージ減少レート（1秒あたり）
    
    private Coroutine feverGaugeDecreaseCoroutine; // ゲージ減少コルーチン
    // ゲーム開始時に呼ばれる
    void Start()
    {
        // FPSを60に固定
        Application.targetFrameRate = 60;
        
        // スコアの初期化
        currentScore = 0;
        UpdateScoreText();

        // --- タイマーの初期化 ---
        currentTime = timeLimit;
        isGameActive = true;

        // ゲーム開始時にBGMを開始
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.StartNormalBGM();
            Debug.Log("ゲーム開始 - 通常BGMを開始");
        }

        // ゲージの初期化（幅0に）
        if (comboGauge != null)
        {
            var rect = comboGauge.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0f, rect.sizeDelta.y);
        }

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

        // タイマーが一時停止中でなければ時間を減らしていく
        if (!isTimerPaused)
        {
            currentTime -= Time.deltaTime;
        }

        // もし残り時間が0以下になったら
        if (currentTime <= 0)
        {
            currentTime = 0; // マイナス表示を防ぐ
            isGameActive = false; // ゲームを非アクティブにする
            Debug.Log("ゲーム終了！");
            
            // ゲーム終了時にすべてのBGMを完全停止
            if (BGMManager.Instance != null)
            {
                BGMManager.Instance.StopNormalBGM();
                BGMManager.Instance.StopFeverBGM();
                Debug.Log("ゲーム終了 - すべてのBGMを停止");
            }
            
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
        Debug.Log(currentScore);
    }

    // UIテキストを更新するためのメソッド
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
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
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
        
        if (preEffectVideoPlayer != null)
        {
            preEffectVideoPlayer.Prepare();
        }
        
        // Singletonの設定
        Instance = this;
    }

    // コンボ成立時にゲージを増やす
    public void AddCombo(int combo){
        // フィーバーモード中はゲージ増加を停止
        if (isFeverActive)
        {
            Debug.Log("フィーバーモード中のためゲージ増加をスキップ");
            return;
        }
        
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
            StartPreEffectVideo();
        }

        yield return null;
    }

    // 透過動画開始（フィーバー前エフェクト）
    private void StartPreEffectVideo()
    {
        if (preEffectVideoPlayer == null || preEffectVideoUI == null)
        {
            Debug.LogWarning("PreEffectVideoPlayerまたはPreEffectVideoUIが設定されていません - 直接フィーバーモードに移行");
            StartFeverMode();
            return;
        }

        Debug.Log("透過エフェクト動画開始！");

        // 透過動画中はタイマーを一時停止
        isTimerPaused = true;
        Debug.Log("タイマー一時停止（透過動画）");

        // BGMを一時停止
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PauseNormalBGM();
            Debug.Log("BGM一時停止（透過動画）");
        }

        // スポーンを停止
        TargetSpawner spawner = FindFirstObjectByType<TargetSpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
            Debug.Log("スポーン停止（透過動画）");
        }

        // 透過動画UIを表示
        preEffectVideoUI.SetActive(true);

        // VideoPlayerの設定を確実にしてから再生
        if (preEffectVideoPlayer != null)
        {
            preEffectVideoPlayer.isLooping = false; // ループしないように設定
            preEffectVideoPlayer.Play();
        }

        // 透過動画終了時のコールバックを設定
        preEffectVideoPlayer.loopPointReached -= OnPreEffectVideoEnd; // 重複登録防止
        preEffectVideoPlayer.loopPointReached += OnPreEffectVideoEnd;
    }

    // 透過動画終了時の処理
    private void OnPreEffectVideoEnd(VideoPlayer vp)
    {
        Debug.Log("透過エフェクト動画終了 - フィーバー動画開始");

        // 透過動画UIを非表示
        preEffectVideoUI.SetActive(false);

        // コールバック解除
        preEffectVideoPlayer.loopPointReached -= OnPreEffectVideoEnd;

        // フィーバー動画に移行
        StartFeverMode();
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

        // フィーバー動画中はタイマーを一時停止
        isTimerPaused = true;
        Debug.Log("タイマー一時停止");

        // 動画中は全てのBGMを一時停止
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.PauseNormalBGM();
            BGMManager.Instance.StopFeverBGM(); // フィーバーBGMはそもそも再生中ではない
        }

        // 動画UIを表示
        feverVideoUI.SetActive(true);

        // VideoPlayerの設定を確実にしてから再生
        if (feverVideoPlayer != null)
        {
            feverVideoPlayer.isLooping = false; // ループしないように設定
            feverVideoPlayer.Play();
        }

        // 動画終了時のコールバックを設定（既存のコールバックを一度クリア）
        feverVideoPlayer.loopPointReached -= OnFeverVideoEnd; // 重複登録防止
        feverVideoPlayer.loopPointReached += OnFeverVideoEnd;
        
        // TargetSpawnerにフィーバー開始を通知
        TargetSpawner spawner = FindFirstObjectByType<TargetSpawner>();
        if (spawner != null)
        {
            spawner.StartFeverMode();
        }
        
        // コンボゲージ減少は動画終了後に開始（動画終了コールバックで開始）
    }

    // 動画終了時の処理
    private void OnFeverVideoEnd(VideoPlayer vp)
    {
        Debug.Log("フィーバー動画終了 - ゲージ減少開始");

        // フィーバー動画終了後にタイマーを再開
        isTimerPaused = false;
        Debug.Log("タイマー再開");

        // 動画UIを非表示
        feverVideoUI.SetActive(false);

        // コールバックを解除
        feverVideoPlayer.loopPointReached -= OnFeverVideoEnd;

        // 動画終了後にフィーバーBGMを再生し、ゲージ減少開始
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.StartFeverBGM();
        }
        
        feverGaugeDecreaseCoroutine = StartCoroutine(DecreaseComboGaugeDuringFever());
        
        // TargetSpawnerにfemale-gorilla生成開始を通知
        TargetSpawner spawner = FindFirstObjectByType<TargetSpawner>();
        if (spawner != null)
        {
            spawner.StartFemaleGorillaSpawn();
        }
    }

    // フィーバーモードを手動で停止する場合のメソッド
    public void StopFeverMode()
    {
        // 透過動画再生中の場合
        if (preEffectVideoPlayer != null && preEffectVideoPlayer.isPlaying)
        {
            preEffectVideoPlayer.Stop();
            OnPreEffectVideoEnd(preEffectVideoPlayer);
        }
        
        // フィーバー動画再生中の場合
        if (feverVideoPlayer != null && feverVideoPlayer.isPlaying)
        {
            feverVideoPlayer.Stop();
            OnFeverVideoEnd(feverVideoPlayer);
        }
    }


    
    // フィーバーモード終了処理
    private void EndFeverMode()
    {
        isFeverActive = false;
        
        // ゲージ減少を停止
        if (feverGaugeDecreaseCoroutine != null)
        {
            StopCoroutine(feverGaugeDecreaseCoroutine);
            feverGaugeDecreaseCoroutine = null;
        }
        
        // フィーバータイム終了時にBGMを元に戻す
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.EndFeverMode();
        }
        
        // TargetSpawnerにフィーバー終了を通知
        TargetSpawner spawner = FindFirstObjectByType<TargetSpawner>();
        if (spawner != null)
        {
            spawner.EndFeverMode();
        }
    }
    
    
    // フィーバータイム中のコンボゲージ減少
    private IEnumerator DecreaseComboGaugeDuringFever()
    {
        while (isFeverActive)
        {
            // ゲージの幅を減少
            Vector2 nowSize = comboGauge.GetComponent<RectTransform>().sizeDelta;
            float decreaseAmount = combo1 * comboGaugeDecreaseRate * Time.deltaTime;
            nowSize.x -= decreaseAmount;
            
            // 0以下になったらフィーバー終了
            if (nowSize.x <= 0)
            {
                nowSize.x = 0;
                currentCombo = 0;
                comboGauge.GetComponent<RectTransform>().sizeDelta = nowSize;
                
                Debug.Log("ゲージが完全に0に到達 - フィーバー終了");
                // ゲージが0になったのでフィーバー終了
                EndFeverMode();
                break;
            }
            
            // ゲージに反映
            comboGauge.GetComponent<RectTransform>().sizeDelta = nowSize;
            
            // コンボ数を更新
            currentCombo = Mathf.RoundToInt(nowSize.x / combo1);
            
            yield return null;
        }
    }
}
