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
        m5StickReader = M5StickReader.Instance;

        if (m5StickReader == null)
        {
            Debug.LogError("M5StickReaderが見つかりません。シーンに配置されていない可能性があります。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m5StickReader.ConsumeThrowActionFlag() && !m5StickReader.getThrowedActionFlag())
        {
            Vector2 worldPoint = m5StickReader.getThrowPos();

            confirmationtTargetHitSenser(worldPoint);
            m5StickReader.setThrowedActionFlag(true);
        }
        else
        {
            m5StickReader.setThrowedActionFlag(false);
            m5StickReader.SendFlag(false);
        }

        Debug.Log("x: " + m5StickReader.getThrowPos().x + " y: " + m5StickReader.getThrowPos().y + " throw: " + m5StickReader.getThrowedActionFlag());
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