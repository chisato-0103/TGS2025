using UnityEngine;

public class RankingData
{
    GameManager gm = new GameManager();
    public int score = 0;
    public int[] rscore = new int[10];
    public string[] Pname = new string[10];
    public RankingData(GameManager gm)
    {
        this.gm = gm;
        this.score = gm.getCurrentScore();
        for (int i = 0; i < 10; i++)
        {
            rscore[i] = 0;
            Pname[i] = "NoData";
        }
    }

    public void RankingChange()
    {
        this.score = gm.getCurrentScore();
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
