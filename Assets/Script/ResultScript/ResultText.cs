using UnityEngine;
using TMPro;

public class ResultText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI SResultText;
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
            // スコアに応じて画像を変更
            if (sr.score >= sr.high)
            {
                SResultText.text = "Parfect!";
            }
            else if (sr.score >= sr.middle)
            {
                SResultText.text = "Good";
            }
            else
            {
                SResultText.text = "Bad";
            }
            flag = true;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
