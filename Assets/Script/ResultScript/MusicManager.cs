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

    void Start()
    {
        sr = FindFirstObjectByType<Score>(); //ScoreScriptにあるscoreを使うためにオブジェクト検索をかけている
        High = sr.high;
        Middle = sr.middle;
    }

    void Update()
    {
        if (sr.time >= 5.0)
        {
            if (sr.score == 1)
            {
                StartCoroutine(DelayedAction(1f));
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
        if (audioSource.clip == clip) return; // すでに再生中なら何もしない
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    IEnumerator DelayedAction(float delay)
    {
        yield return new WaitForSeconds(delay);  // delay 秒待つ
        Debug.Log("待機が終わった後の処理！");
        PlayBGM(TrueBGM);
    }
}
