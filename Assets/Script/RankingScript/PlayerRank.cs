using UnityEngine;
using UnityEngine.UI; 

public class PlayerRank : MonoBehaviour
{
    public Text PRankText;
    RankingDataOld rd = new RankingDataOld();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int score = GameManager.currentScore;
        int PRank = -1;
        if (score > RankingDataOld.rscore[4])
        {
            int i;
            PRank = 5;
            // 自分の順位を特定する手順
            for (i = 3; i >= 0 && RankingDataOld.rscore[i] < score; i--)
            {
                PRank -= 1;
            }
        }
        /*if (PRank == -1)
        {
            PRankText.text = "あなたの得点は" + score + "でランキング外でした。";
        }
        else
        {
            PRankText.text = "あなたの得点は" + score + "で順位は" + PRank + "位でした。";
        }*/
        PRankText.text = score + "匹！";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
