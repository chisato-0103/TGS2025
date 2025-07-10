using UnityEngine;
using UnityEngine.UI; 

public class ResultTextOld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Text SResultText;
    Score sr;
    float time = 0.0f;
    bool flag = false;
    void Start()
    {
        sr = FindFirstObjectByType<Score>(); //ScoreScriptにあるscoreを使うためにオブジェクト検索をかけている
        SResultText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 5.0 && flag == false)
        {
            // スコアに応じて言葉を変更
            if (sr.score >= sr.high)
            {
                SResultText.text = "すごい！";
            }
            else if (sr.score >= sr.middle)
            {
                SResultText.text = "いいね！";
            }
            else
            {
                SResultText.text = "頑張った";
            }
            flag = true;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}

