using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI ScoreText;
    public float score = 0.0f;
    float start = 0.0f; //今の時間を記録する
    float end = 3.0f; //終了時刻
    public float high = 800.0f, middle = 500.0f; //外部からもいじれるようにスコアの判定ラインをパブリックにする
    void Start()
    {
        score = Random.Range(0.0f, 1000.0f);
        //score = GameManager.currentScore;
        ScoreText.text = ""; //ここでテキストの中身を初期化
    }

    // Update is called once per frame
    void Update()
    {
        if (start <= end)
        {
            ScoreText.text = (score * start / end).ToString("F0"); //スコアが徐々に増加するような表示をここで行っている
            start += Time.deltaTime; //ここで現在の時間を進めている
        }
        else
        {
            ScoreText.text = score.ToString("F0"); //表示スコアが不足、または大きい場合に備えてここでscoreの値を入れている
        }
    }
}