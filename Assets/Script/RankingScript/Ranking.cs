using UnityEngine;
using TMPro;

public class Ranking : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RankingData rd = new RankingData();
    public TextMeshProUGUI RankingText;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            rd.RankingChange();
        }
        RankingText.text = "";
        int count = 1;
        for (int i = 0; i < 10; i++)
        {
            if (i != 0 && rd.rscore[i] == rd.rscore[i - 1])
            {
                RankingText.text += "Ranking:" + count + "   PlayerName:" + rd.Pname[i] + " Score:" + rd.rscore[i] + "\n";
            }
            else
            {
                if (i != 0)
                {
                    count++;
                }
                if (count >= 10)
                {
                    RankingText.text += "Ranking:" + count + " PlayerName:" + rd.Pname[i] + " Score:" + rd.rscore[i] + "\n";
                }
                else
                {
                    RankingText.text += "Ranking:" + count + "   PlayerName:" + rd.Pname[i] + " Score:" + rd.rscore[i] + "\n";
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
