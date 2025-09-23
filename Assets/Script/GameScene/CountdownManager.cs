using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownManager : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField]
    private TextMeshProUGUI countdownText;
    [SerializeField]
    private GameObject countdownUI;

    [Header("開始カウント設定")]
    [SerializeField]
    private AudioClip[] startCountdownClips; // 3, 2, 1, start の順番
    [SerializeField]
    private string[] startCountdownTexts = {"3", "2", "1", "ゲーム開始！"};

    [Header("終了カウント設定")]
    [SerializeField]
    private AudioClip[] endCountdownClips; // 10, 9, 8...1, end の順番
    [SerializeField]
    private string[] endCountdownTexts = {"10", "9", "8", "7", "6", "5", "4", "3", "2", "1", "ゲーム終了！"};

    [Header("表示設定")]
    [SerializeField]
    private float countInterval = 1.0f;
    [SerializeField]
    private float textDisplayDuration = 0.8f;

    public static CountdownManager Instance;

    private bool isCountdownActive = false;

    void Awake()
    {
        Instance = this;

        if (countdownUI != null)
        {
            countdownUI.SetActive(false);
        }
    }

    public void StartGameCountdown(System.Action onComplete)
    {
        if (isCountdownActive) return;

        StartCoroutine(PlayCountdown(startCountdownClips, startCountdownTexts, onComplete));
    }

    public void StartEndCountdown(System.Action onComplete)
    {
        if (isCountdownActive) return;

        StartCoroutine(PlayEndCountdown(endCountdownClips, endCountdownTexts, onComplete));
    }

    private IEnumerator PlayCountdown(AudioClip[] clips, string[] texts, System.Action onComplete)
    {
        isCountdownActive = true;

        if (countdownUI != null)
        {
            countdownUI.SetActive(true);
        }

        for (int i = 0; i < texts.Length; i++)
        {
            if (countdownText != null)
            {
                countdownText.text = texts[i];
            }

            if (i < clips.Length && clips[i] != null)
            {
                if (BGMManager.Instance != null)
                {
                    BGMManager.Instance.PlayCountdownSound(clips[i]);
                }
            }

            yield return new WaitForSeconds(countInterval);
        }

        if (countdownText != null)
        {
            countdownText.text = "";
        }

        if (countdownUI != null)
        {
            countdownUI.SetActive(false);
        }

        isCountdownActive = false;
        onComplete?.Invoke();
    }

    // 終了カウントダウン専用のコルーチン（テキスト表示なし）
    private IEnumerator PlayEndCountdown(AudioClip[] clips, string[] texts, System.Action onComplete)
    {
        isCountdownActive = true;

        // 終了カウントダウンではUIを表示しない
        // countdownUI は表示しない

        for (int i = 0; i <= 10; i++) // 10秒から0秒まで
        {
            // タイマー表示を更新（10から0まで）
            int remainingTime = 10 - i;
            UpdateGameManagerTimer(remainingTime);

            // 音声再生（配列範囲内の場合のみ）
            if (i < clips.Length && clips[i] != null)
            {
                if (BGMManager.Instance != null)
                {
                    BGMManager.Instance.PlayCountdownSound(clips[i]);
                }
            }

            yield return new WaitForSeconds(countInterval);
        }

        isCountdownActive = false;
        onComplete?.Invoke();
    }

    // GameManagerのタイマー表示を更新
    private void UpdateGameManagerTimer(int seconds)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetTimerDisplay(seconds);
        }
    }

    public bool IsCountdownActive()
    {
        return isCountdownActive;
    }

    public void StopCountdown()
    {
        StopAllCoroutines();

        if (countdownUI != null)
        {
            countdownUI.SetActive(false);
        }

        if (countdownText != null)
        {
            countdownText.text = "";
        }

        isCountdownActive = false;
    }
}