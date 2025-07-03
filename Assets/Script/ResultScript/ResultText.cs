using UnityEngine;
using TMPro;

public class ResultText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI SResultText;
    Score sr;
    float time = 0.0f; //時間計測用の変数
    bool flag = false; //４秒を過ぎた後に繰り返させないための変数
    void Start()
    {
        sr = FindFirstObjectByType<Score>(); //Score Scriptに入っているScoreの点数を持ってくる
        SResultText.text = ""; //一旦テキストの中身を空に
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 4.0 && flag == false)
        {
            // スコアに応じてテキストを変更
            if (sr.score >= 800.0f)
            {
                SResultText.text = "Parfect!";
            }
            else if (sr.score >= 500.0f)
            {
                SResultText.text = "Good";
            }
            else
            {
                SResultText.text = "Bad";
            }
            flag = true;//2回目を繰り返さないためにflagをtrueに
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
