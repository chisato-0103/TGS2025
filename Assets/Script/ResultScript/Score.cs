using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI ScoreText;
    public float score = 0.0f; //点数を記憶するための変数
    float start = 0.0f;//今の時間を格納するための変数
    float end = 3.0f;//終了時間を格納するための変数
    void Start()
    {
        score = Random.Range(0.0f, 1000.0f);
        //score = GameManager.currentScore;
        ScoreText.text = ""; //ここでテキストを初期化
    }

    // Update is called once per frame
    void Update()
    {
        if (start <= end) //starがendの数値を超すまで繰り返す
        {
            ScoreText.text = (score * start / end).ToString("F0"); //点数を(今の時間/終了時刻)でかけると終了時刻まで段々と点数が増えるようになる。
                                                                　 // また"F0"は小数点を切り捨てるためにある。
            start += Time.deltaTime;
        }
        else
        {
            ScoreText.text = score.ToString("F0"); //一応得点にズレがないように最終的にscoreで表示している。
        }
    }
}