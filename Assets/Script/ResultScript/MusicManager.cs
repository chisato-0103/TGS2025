using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;   // 再生用
    public AudioClip LowBGM;          // 低スコア用BGM
    public AudioClip MiddleBGM;       // 普通スコア
    public AudioClip HighBGM;         // 高スコア
    public AudioClip TrueBGM;         // １匹だった時
    private Score sr;
    private float High = 0, Middle = 0, Score = 0;
    private bool bgmStarted = false;  // BGM再生開始フラグ

    public static MusicManager Instance;  // Singletonインスタンス

    void Awake()
    {
        // Singletonパターンの実装
        if (Instance == null)
        {
            Instance = this;

            // 親からの依存を断ち切ってからDontDestroyOnLoadを実行
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            DontDestroyOnLoad(gameObject);  // シーン切り替え時も保持
            Debug.Log("MusicManager Singleton作成 - DontDestroyOnLoad設定");
        }
        else
        {
            Debug.Log("MusicManager重複インスタンス検出 - 破棄");
            Destroy(gameObject);  // 既にインスタンスが存在する場合は破棄
            return;
        }
    }

    void Start()
    {
        // Singletonが設定されていない場合はここで設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MusicManager Singleton作成 - Start内でDontDestroyOnLoad設定");
        }

        // Scoreコンポーネントが存在する場合のみ初期化
        sr = FindFirstObjectByType<Score>();
        if (sr != null)
        {
            High = sr.high;
            Middle = sr.middle;
            Debug.Log("Score コンポーネントを発見 - BGM選択準備完了");
        }
        else
        {
            Debug.Log("Score コンポーネントが見つからない - ランキングシーンまたは他のシーン");

            // ランキングシーンでのAudioSource状態確認
            if (audioSource != null)
            {
                Debug.Log($"ランキングシーン AudioSource状態: 再生中={audioSource.isPlaying}, クリップ={audioSource.clip?.name}");
            }
            else
            {
                Debug.LogError("AudioSourceがnullです");
            }
        }
    }

    void Update()
    {
        // Scoreコンポーネントが存在し、まだBGMを開始していない場合のみ処理
        if (sr != null && !bgmStarted && sr.time >= 5.0)
        {
            bgmStarted = true;  // BGM開始フラグを設定

            if (sr.score == 1)
            {
                StartCoroutine(DelayedAction(2f));
            }
            else if (sr.score >= High)
            {
                PlayBGM(HighBGM);
            }
            else if (sr.score >= Middle)
            {
                PlayBGM(MiddleBGM);
            }
            else
            {
                PlayBGM(LowBGM);
            }
            Debug.Log("BGM開始");
        }
    }

    // BGM再生用の共通関数
    void PlayBGM(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceが設定されていません");
            return;
        }

        if (clip == null)
        {
            Debug.LogError("BGM AudioClipが設定されていません");
            return;
        }

        if (audioSource.clip == clip)
        {
            Debug.Log("同じBGMが既に再生中 - スキップ");
            return; // すでに再生中なら何もしない
        }

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
        Debug.Log($"BGM再生開始: {clip.name}");
    }

    IEnumerator DelayedAction(float delay)
    {
        yield return new WaitForSeconds(delay);  // delay 秒待つ
        Debug.Log("待機が終わった後の処理！");
        PlayBGM(TrueBGM);
        bgmStarted = true;  // BGM開始フラグを設定
    }

    // BGMを停止するメソッド（タイトル画面遷移時に使用）
    public void StopBGM()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("リザルトBGMを停止");
        }
    }

    void OnDestroy()
    {
        Debug.Log("MusicManager破棄されました");
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
