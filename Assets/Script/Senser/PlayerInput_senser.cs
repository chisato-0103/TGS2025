using UnityEngine;

public class PlayerInput_senser : MonoBehaviour
{
    // GameManagerへの参照を保持する変数
    private GameManager gameManager;
    // M5StickReaderへの参照を保持する変数
    private M5StickReader m5StickReader;

    // StartメソッドでGameManagerを一度だけ探して保持しておく
    void Start()
    {
        // シーン内からGameManagerコンポーネントを探してくる
        gameManager = FindFirstObjectByType<GameManager>();
        // シーン内からM5StickReaderコンポーネントを探してくる
        m5StickReader = FindFirstObjectByType<M5StickReader>();
    }

    void Update()
    {
        if (m5StickReader.getThrowActionflag() && !m5StickReader.getThrowedActionFlag())
        {
            Vector2 worldPoint = m5StickReader.getThrowPos();

            confirmationtTargetHitSenser(worldPoint);
            m5StickReader.setThrowedActionFlag(true);

            // フラグをリセット
            m5StickReader.resetThrowFlag();
        }
        else
        {
            m5StickReader.setThrowedActionFlag(false);
            m5StickReader.SendFlag(false);
        }
        Debug.Log("x: " + m5StickReader.getThrowPos().x + " y: " + m5StickReader.getThrowPos().y + " throw: " + m5StickReader.getThrowedActionFlag());
    }

    //ターゲットに当たったかを処理する関数
    private void confirmationtTargetHitMouse(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            Target target = hit.collider.GetComponent<Target>();
            if (target != null)
            {
                // 加点・減点判定
                if (gameManager != null)
                {
                    gameManager.AddScore(target.points);
                    if (target.points > 0)
                    {
                        // 加点時はコンボ加算
                        gameManager.AddCombo(1);
                    }
                    else
                    {
                        // 減点時はコンボリセット＋ゲージ半分
                        gameManager.ResetComboAndHalveGauge();
                    }
                }

                target.Hit();
            }
        }
    }

    private void confirmationtTargetHitSenser(Vector2 worldPoint)
    {
        float hitRadius = 1.2f; // 半径は調整可能
        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPoint, hitRadius);

        foreach (Collider2D col in hits)
        {
            Target target = col.GetComponent<Target>();
            if (target != null)
            {
                if (gameManager != null)
                {
                    gameManager.AddScore(target.points);
                    if (target.points > 0)
                    {
                        gameManager.AddCombo(1);
                    }
                    else
                    {
                        gameManager.ResetComboAndHalveGauge();
                    }
                }
                target.Hit();
            }
        }
        m5StickReader.setPower(0.0f);
    }
}
