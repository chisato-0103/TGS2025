using UnityEngine;
using UnityEngine.UI;

public class GoriraImage1 : MonoBehaviour
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
        sr = FindFirstObjectByType<Score>();

    }
    void Update()
    {
        if (time >= 9.0 && flag == false)
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
            flag = true;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
}
