using UnityEngine;
using UnityEngine.UI; 

public class RankingOld : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RankingDataOld rd = new RankingDataOld();
    public Text RankingText;

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
                RankingText.text += count  + "位:" + "     " + rd.Pname[i] +  "     " + rd.rscore[i] + "点" + "\n";
            }
            else
            {
                if (i != 0)
                {
                    count++;
                }
                if (count >= 10)
                {
                    RankingText.text += count  + "位:" + "   " + rd.Pname[i] +  "     " + rd.rscore[i] + "点" + "\n";
                }
                else
                {
                    RankingText.text += count  + "位:" + "     " + rd.Pname[i] +  "     " + rd.rscore[i] + "点" + "\n";
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
