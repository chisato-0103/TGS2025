using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource normalBgmAudioSource; // 通常BGM用のAudioSource
    [SerializeField]
    private AudioSource feverBgmAudioSource; // フィーバーBGM用のAudioSource
    [SerializeField]
    private AudioSource countdownAudioSource; // カウントダウン用のAudioSource
    [SerializeField]
    private AudioSource seAudioSource; // 効果音用のAudioSource
    [SerializeField]
    private AudioClip feverSoundEffect; // フィーバー効果音のAudioClip

    public static BGMManager Instance;

    void Awake()
    {
        // Singletonパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ビルド時のデバッグ用：AudioSourceの初期化確認
            ValidateAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // AudioSourceの初期化状態を確認
    private void ValidateAudioSources()
    {
        Debug.Log("=== BGMManager AudioSource 初期化確認 ===");

        if (normalBgmAudioSource == null)
            Debug.LogError("normalBgmAudioSource が設定されていません！");
        else
            Debug.Log($"normalBgmAudioSource OK - Clip: {(normalBgmAudioSource.clip != null ? normalBgmAudioSource.clip.name : "None")}");

        if (feverBgmAudioSource == null)
            Debug.LogError("feverBgmAudioSource が設定されていません！");
        else
            Debug.Log($"feverBgmAudioSource OK - Clip: {(feverBgmAudioSource.clip != null ? feverBgmAudioSource.clip.name : "None")}");

        if (countdownAudioSource == null)
            Debug.LogError("countdownAudioSource が設定されていません！");
        else
            Debug.Log($"countdownAudioSource OK");

        if (seAudioSource == null)
            Debug.LogError("seAudioSource が設定されていません！");
        else
            Debug.Log($"seAudioSource OK - フィーバー効果音: {(feverSoundEffect != null ? feverSoundEffect.name : "None")}");

        Debug.Log("=== BGMManager 初期化確認完了 ===");
    }

    void Start()
    {
        // ゲーム開始時にBGMは自動開始しない
        // GameManagerのStartActualGame()から呼び出される
    }

    // 通常BGMを開始
    public void StartNormalBGM()
    {
        if (normalBgmAudioSource != null && normalBgmAudioSource.clip != null)
        {
            // ビルド環境でのトラブル回避のため、明示的にAudioSourceを有効化
            normalBgmAudioSource.enabled = true;
            normalBgmAudioSource.volume = Mathf.Clamp01(normalBgmAudioSource.volume);
            normalBgmAudioSource.Play();
            Debug.Log($"通常BGM開始: {normalBgmAudioSource.clip.name} - Volume: {normalBgmAudioSource.volume} - Playing: {normalBgmAudioSource.isPlaying}");
        }
        else
        {
            Debug.LogError($"通常BGM再生失敗 - AudioSource: {normalBgmAudioSource != null} - AudioClip: {(normalBgmAudioSource != null && normalBgmAudioSource.clip != null)}");
        }
    }

    // 通常BGMを一時停止
    public void PauseNormalBGM()
    {
        if (normalBgmAudioSource != null && normalBgmAudioSource.isPlaying)
        {
            normalBgmAudioSource.Pause();
            Debug.Log("通常BGM一時停止");
        }
    }

    // 通常BGMを再開
    public void ResumeNormalBGM()
    {
        if (normalBgmAudioSource != null && !normalBgmAudioSource.isPlaying)
        {
            normalBgmAudioSource.UnPause();
            Debug.Log("通常BGM再開");
        }
    }

    // 通常BGMを停止
    public void StopNormalBGM()
    {
        if (normalBgmAudioSource != null)
        {
            normalBgmAudioSource.Stop();
            Debug.Log("通常BGM停止");
        }
    }

    // フィーバーBGMを開始
    public void StartFeverBGM()
    {
        if (feverBgmAudioSource != null && feverBgmAudioSource.clip != null)
        {
            feverBgmAudioSource.Play();
            Debug.Log("フィーバーBGM開始: " + feverBgmAudioSource.clip.name);
        }
        else
        {
            Debug.LogWarning("フィーバーBGM AudioSourceまたはAudioClipが設定されていません");
        }
    }

    // フィーバーBGMを停止
    public void StopFeverBGM()
    {
        if (feverBgmAudioSource != null && feverBgmAudioSource.isPlaying)
        {
            feverBgmAudioSource.Stop();
            Debug.Log("フィーバーBGM停止");
        }
    }

    // フィーバーモード開始（通常BGM一時停止 + フィーバーBGM開始）
    public void StartFeverMode()
    {
        PauseNormalBGM();
        StartFeverBGM();
    }

    // フィーバーモード終了（フィーバーBGM停止 + 通常BGM再開）
    public void EndFeverMode()
    {
        StopFeverBGM();
        ResumeNormalBGM(); // 一時停止からの復帰なので途中から再開される
    }

    // BGM音量設定
    public void SetNormalBGMVolume(float volume)
    {
        if (normalBgmAudioSource != null)
        {
            normalBgmAudioSource.volume = Mathf.Clamp01(volume);
        }
    }

    public void SetFeverBGMVolume(float volume)
    {
        if (feverBgmAudioSource != null)
        {
            feverBgmAudioSource.volume = Mathf.Clamp01(volume);
        }
    }

    // カウントダウン音声を再生
    public void PlayCountdownSound(AudioClip clip)
    {
        if (countdownAudioSource != null && clip != null)
        {
            // ビルド環境での音声再生を確実にするため、設定を明示的に行う
            countdownAudioSource.enabled = true;
            countdownAudioSource.volume = Mathf.Clamp01(countdownAudioSource.volume);
            countdownAudioSource.clip = clip;
            countdownAudioSource.Play();
            Debug.Log($"カウントダウン音声再生: {clip.name} - Volume: {countdownAudioSource.volume} - Playing: {countdownAudioSource.isPlaying}");

            // 再生が開始されない場合のフォールバック
            if (!countdownAudioSource.isPlaying)
            {
                Debug.LogWarning("カウントダウン音声の再生が開始されませんでした。PlayOneShot を試行します。");
                countdownAudioSource.PlayOneShot(clip);
            }
        }
        else
        {
            Debug.LogError($"カウントダウン音声再生失敗 - AudioSource: {countdownAudioSource != null} - AudioClip: {clip != null}");
        }
    }

    // カウントダウン音声を停止
    public void StopCountdownSound()
    {
        if (countdownAudioSource != null && countdownAudioSource.isPlaying)
        {
            countdownAudioSource.Stop();
            Debug.Log("カウントダウン音声停止");
        }
    }

    // カウントダウン音量設定
    public void SetCountdownVolume(float volume)
    {
        if (countdownAudioSource != null)
        {
            countdownAudioSource.volume = Mathf.Clamp01(volume);
        }
    }

    // フィーバー効果音を再生
    public void PlayFeverSoundEffect()
    {
        if (seAudioSource != null && feverSoundEffect != null)
        {
            // ビルド環境での効果音再生を確実にする
            seAudioSource.enabled = true;
            seAudioSource.volume = Mathf.Clamp01(seAudioSource.volume);
            seAudioSource.PlayOneShot(feverSoundEffect);
            Debug.Log($"フィーバー効果音再生: {feverSoundEffect.name} - Volume: {seAudioSource.volume}");
        }
        else
        {
            Debug.LogError($"フィーバー効果音再生失敗 - SEAudioSource: {seAudioSource != null} - FeverSE: {feverSoundEffect != null}");
        }
    }

    // 指定した効果音を再生
    public void PlaySoundEffect(AudioClip clip)
    {
        if (seAudioSource != null && clip != null)
        {
            seAudioSource.PlayOneShot(clip);
            Debug.Log("効果音再生: " + clip.name);
        }
        else
        {
            Debug.LogWarning("効果音 AudioSourceまたはAudioClipが設定されていません");
        }
    }

    // 効果音音量設定
    public void SetSEVolume(float volume)
    {
        if (seAudioSource != null)
        {
            seAudioSource.volume = Mathf.Clamp01(volume);
        }
    }
}