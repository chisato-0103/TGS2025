using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI ScoreText;
    public float score = 0.0f;
    float start = 0.0f;
    float end = 3.0f;
    void Start()
    {
        score = Random.Range(0.0f, 1000.0f);
        //score = GameManager.currentScore;
        ScoreText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (start <= end)
        {
            ScoreText.text = (score * start / end).ToString("F0");
            start += Time.deltaTime;
        }
        else
        {
            ScoreText.text = score.ToString("F0");
        }
    }
}