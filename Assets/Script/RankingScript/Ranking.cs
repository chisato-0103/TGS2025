using UnityEngine;
using TMPro;

public class Ranking : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameManager gm = new GameManager();
    public TextMeshProUGUI RankingText;
    void Start()
    {
        RankingText.text = "";
        int score = gm.getCurrentScore();
        for (int i = 0; i < 10; i++)
        {
            RankingText.text += "Ranking:" + (i + 1) + "    Score:" + score + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
