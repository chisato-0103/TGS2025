using UnityEngine;
using UnityEngine.UI;

public class GoriraImage : MonoBehaviour
{
    Score sr;
    public Image ResultImage;         // 差し替え対象のImage
    public Sprite loseGorira;         // スコアが低いときの画像
    public Sprite Gorira;             // 中間
    public Sprite WinGorira;          // 高スコア
    public float time = 0.0f;

    bool flag = false;
    void Start()
    {
        sr = FindFirstObjectByType<Score>(); //ScoreScriptにあるscoreを使うためにオブジェクト検索をかけている

    }
    void Update()
    {
        if (time >= 4.0 && flag == false)
        {
            // スコアに応じて画像を変更
            if (sr.score >= sr.high)
            {
                ResultImage.sprite = WinGorira;
            }
            else if (sr.score >= sr.middle)
            {
                ResultImage.sprite = Gorira;
            }
            else
            {
                ResultImage.sprite = loseGorira;
            }
            flag = true;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
