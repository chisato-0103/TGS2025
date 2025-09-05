using UnityEngine;
using UnityEngine.UI;

public class GoriraImage : MonoBehaviour
{
    Score sr;
    public Image ResultImage;         // 差し替え対象のImage
    public Sprite loseGorira;         // スコアが低いときの画像
    public Sprite Gorira;             // 中間
    public Sprite WinGorira;          // 高スコア
    public Sprite TrueGorira;          // 一匹の時の画像
    private FadeManager fade;

    bool flag = false;
    void Start()
    {
        sr = FindFirstObjectByType<Score>(); //ScoreScriptにあるscoreを使うためにオブジェクト検索をかけている
        fade = FindObjectOfType<FadeManager>();

    }
    void Update()
    {
        if (sr.time >= 5.0 && flag == false)
        {
            // スコアに応じて画像を変更
            if (sr.score == 1)
            {
                //フェードアウト開始
                StartCoroutine(fade.FadeOut(() =>
                {
                    // フェードアウト終了後に画像切り替え
                    ResultImage.sprite = TrueGorira;

                    // 画像切り替え後フェードイン開始
                    StartCoroutine(fade.FadeIn(null));
                }));
            }
            else if (sr.score >= sr.high)
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
    }
}
