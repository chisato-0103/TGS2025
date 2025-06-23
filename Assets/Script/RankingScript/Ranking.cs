using UnityEngine;
using TMPro;

public class Ranking : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameManager gm = new GameManager();
    RankingData rd;
    public TextMeshProUGUI RankingText;
    void Awake()
    {
        rd = new RankingData(gm);
    }
    void Start()
    {
        rd.RankingChange();
        RankingText.text = "";
        //int score = gm.getCurrentScore();
        for (int i = 0; i < 10; i++)
        {
            if (i + 1 >= 10)
            {
                RankingText.text += "Ranking:" + (i + 1) + "    PlayerName:" + rd.Pname[i] + "    Score:" + rd.rscore[i] + "\n";
            }
            else
            {
                RankingText.text += "Ranking:" + (i + 1) + "      PlayerName:" + rd.Pname[i] + "    Score:" + rd.rscore[i] + "\n";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
