using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class BtnChange : MonoBehaviour
{
    public Button btn;
    Score sr;
    public bool BtnFlag = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = FindFirstObjectByType<Score>(); //ScoreScriptにあるscoreを使うためにオブジェクト検索をかけている
        btn.onClick.AddListener(BtnTextChange);
        btn.GetComponentInChildren<Text>().text = "リザルト演出をスキップ";
    }

    void Update()
    {
        if (sr.time >= 5.0f)
        {
            btn.GetComponentInChildren<Text>().text = "ランキングシーンに移動";
            BtnFlag = true;
        }
    }

    void BtnTextChange()//ボタンが押された場合の処理
    {
        if (BtnFlag == true)//リザルトは「演出のスキップ」→「ランキングシーンへの移動」の二段階ボタンなので1回目の処理と2回目の処理を分けている。
        {
            ScreenManager screenManager = FindObjectOfType<ScreenManager>();
            if (screenManager != null)
            {
                screenManager.GoToRankingScene();
            }
            else
            {
                Debug.LogError("ScreenManagerが見つかりませんでした");
            }
        }
        else if (BtnFlag == false)
        {
            Debug.Log("ボタンが押されました！");
            btn.GetComponentInChildren<Text>().text = "ランキングシーンに移動";
            BtnFlag = true;
            sr.time = 5.0f;
            sr.start = 4.0f;
        }
        else
        {
            Debug.Log("例外です");
        }
    }
}
