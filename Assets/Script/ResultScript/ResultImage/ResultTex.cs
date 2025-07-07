using UnityEngine;
using TMPro;

public class ResultText1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI SResultText;
    Score sr;
    float time = 0.0f;
    bool flag = false;
    void Start()
    {
        sr = FindFirstObjectByType<Score>();
        SResultText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 4.0 && flag == false)
        {
            // スコアに応じて画像を変更
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
            flag = true;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
