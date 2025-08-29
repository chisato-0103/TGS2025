using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

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
        SceneManager.LoadScene("RankingScene"); // "ResultScene" はリザルト用のシーン名
    }
    
    public void GoToTitleScene()
    {
        Debug.Log("タイトル画面へ遷移");
        SceneManager.LoadScene("TitleScene"); // "ResultScene" はリザルト用のシーン名
    }
}
