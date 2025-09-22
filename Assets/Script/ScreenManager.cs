using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要
using System.Collections;

public class ScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("ゲームスタート！");
        SceneManager.LoadScene("GameScene"); // "GameScene" はゲームプレイ用のシーン名
    }

    // タイトルシーンからストーリーシーンへ遷移するメソッド
    public void GoToStoryScene()
    {
        Debug.Log("ストーリー画面へ遷移");
        SceneManager.LoadScene("StoryScene"); // "StoryScene" はストーリー用のシーン名
    }

    public void GoToLoadScene()
    {
        Debug.Log("ストーリー画面へ遷移");
        SceneManager.LoadScene("Load"); // "StoryScene" はストーリー用のシーン名
    }

    // ゲーム終了時にResultSceneへ遷移するメソッド
    public void GoToResultScene()
    {
        Debug.Log("リザルト画面へ遷移");
        SceneManager.LoadScene("ResultScene"); // "ResultScene" はリザルト用のシーン名
    }

    //リザルトシーンからRankingSceneへ移動するメソッド
    public void GoToRankingScene()
    {
        Debug.Log("ランキンング画面へ遷移");

        // MusicManagerの状態をチェック
        if (MusicManager.Instance != null)
        {
            Debug.Log($"ランキング遷移時 MusicManager存在: AudioSource再生中={MusicManager.Instance.audioSource?.isPlaying}");

            // MusicManagerのGameObjectの親をnullにしてシーンから独立させる
            if (MusicManager.Instance.transform.parent != null)
            {
                MusicManager.Instance.transform.SetParent(null);
                Debug.Log("MusicManagerの親を解除してシーンから独立");
            }
        }
        else
        {
            Debug.Log("ランキング遷移時 MusicManagerが存在しない");
        }

        SceneManager.LoadScene("RankingScene"); // "ResultScene" はリザルト用のシーン名
    }

    public void GoToTitleScene()
    {
        Debug.Log("タイトル画面へ遷移");

        // リザルトBGMを停止
        if (MusicManager.Instance != null)
        {
            Debug.Log("MusicManager.Instanceが見つかりました - BGM停止を実行");
            MusicManager.Instance.StopBGM();
        }
        else
        {
            Debug.Log("MusicManager.Instanceがnullです");
        }

        SceneManager.LoadScene("TitleScene"); // "ResultScene" はリザルト用のシーン名
    }
    
    public void GoToTutorialScene()
    {
        Debug.Log("ストーリー画面へ遷移");
        SceneManager.LoadScene("TutorialScene"); // "TutorialScene" はチューリアル用のシーン名
    }

    IEnumerator CheckMusicManagerAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.5f); // シーンロード完了を待つ

        if (MusicManager.Instance != null)
        {
            Debug.Log($"シーンロード後 MusicManager存在: AudioSource再生中={MusicManager.Instance.audioSource?.isPlaying}");
        }
        else
        {
            Debug.Log("シーンロード後 MusicManagerが存在しない");
        }
    }
}
