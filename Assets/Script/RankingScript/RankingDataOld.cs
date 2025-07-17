using UnityEngine;

public class RankingDataOld
{
    public int score = 0;
    public static int[] rscore = new int[10];
    public static string[] Pname = new string[10];
    public static bool TitleFlag = false;
    public RankingDataOld()
    {
        if (TitleFlag == false)
        {
            for (int i = 0; i < 10; i++)
            {
                rscore[i] = 0;
                Pname[i] = "データなし";
            }
            TitleFlag = true;
        }
        
    }

    public void RankingChange()
    {
        this.score = GameManager.currentScore;
        //this.score = Random.Range(0, 10);
        if (score > rscore[9])
        {
            int i;
            // スコアより小さいものを後ろにずらす
            for (i = 8; i >= 0 && rscore[i] < score; i--)
            {
                rscore[i + 1] = rscore[i];
            }
            // 空いたところにスコアを入れる
            rscore[i + 1] = score;
        }
    }
}
