using UnityEngine;

public class RankingData
{
    public int score = 0;
    public int[] rscore = new int[10];
    public string[] Pname = new string[10];
    public RankingData()
    {
        for (int i = 0; i < 10; i++)
        {
            rscore[i] = 0;
            Pname[i] = "NoData";
        }
    }

    public void RankingChange()
    {
        //this.score = GameManager.currentScore;
        this.score = Random.Range(0, 10);
        if (score > rscore[9])
        {
            rscore[9] = score;
            for (int i = 8; i >= 0; i--)
            {
                if (score < rscore[i])
                {
                    break;
                }
                else
                {
                    rscore[i + 1] = rscore[i];
                    rscore[i] = score;
                }
            }
        }
    }
}
