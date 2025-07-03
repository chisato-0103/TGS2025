using UnityEngine;
using UnityEngine.UI;

public class GoriraImage : MonoBehaviour
{
    Score sr;
    public Image ResultImage;         // 差し替え対象のImage
    public Sprite loseGorira;         // スコアが低いときの画像
    public Sprite Gorira;             // 中間
    public Sprite WinGorira;          // 高スコア

    float time = 0.0f;

    bool flag = false;
    void Start()
    {
        sr = FindFirstObjectByType<Score>(); //Score Scriptにある点数を持ってくるためのもの

    }
    void Update()
    {
        if (time >= 4.0 && flag == false)//4秒を超えるかつ一回も使われたことがないなら動かす
        {
            // スコアに応じて画像を変更
            if (sr.score >= 800.0f)
            {
                ResultImage.sprite = WinGorira;
            }
            else if (sr.score >= 500.0f)
            {
                ResultImage.sprite = Gorira;
            }
            else
            {
                ResultImage.sprite = loseGorira;
            }
            flag = true;//2回目に使われないように一回使われた証を残す
        }
        else
        {
            time += Time.deltaTime;//時間を計測
        }
    }
}
