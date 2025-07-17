using UnityEngine;
using UnityEngine.UI; 

public class RankingOld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RankingDataOld rd = new RankingDataOld();
    public Text RankingText;

    void Start()
    {
        rd.RankingChange();
        RankingText.text = "";
        int count = 1;
        for (int i = 0; i < 10; i++)
        {
            if (i != 0 && RankingDataOld.rscore[i] == RankingDataOld.rscore[i - 1])
            {
                RankingText.text += count  + "位:" + "     " + RankingDataOld.Pname[i] +  "     " + RankingDataOld.rscore[i] + "点" + "\n";
            }
            else
            {
                if (i != 0)
                {
                    count++;
                }
                if (count >= 10)
                {
                    RankingText.text += count  + "位:" + "   " + RankingDataOld.Pname[i] +  "     " + RankingDataOld.rscore[i] + "点" + "\n";
                }
                else
                {
                    RankingText.text += count  + "位:" + "     " + RankingDataOld.Pname[i] +  "     " + RankingDataOld.rscore[i] + "点" + "\n";
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
