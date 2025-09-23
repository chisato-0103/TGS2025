using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource normalBgmAudioSource; // 通常BGM用のAudioSource
    [SerializeField]
    private AudioSource feverBgmAudioSource; // フィーバーBGM用のAudioSource
    [SerializeField]
    private AudioSource countdownAudioSource; // カウントダウン用のAudioSource

    public static BGMManager Instance;

    void Awake()
    {
        // Singletonパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
            normalBgmAudioSource.Play();
            Debug.Log("通常BGM開始: " + normalBgmAudioSource.clip.name);
        }
        else
        {
            Debug.LogWarning("通常BGM AudioSourceまたはAudioClipが設定されていません");
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
            countdownAudioSource.clip = clip;
            countdownAudioSource.Play();
            Debug.Log("カウントダウン音声再生: " + clip.name);
        }
        else
        {
            Debug.LogWarning("カウントダウン AudioSourceまたはAudioClipが設定されていません");
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
}