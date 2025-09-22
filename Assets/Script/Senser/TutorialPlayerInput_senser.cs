using Unity.VisualScripting;
using UnityEngine;

public class TutorialPlayerInput_senser : MonoBehaviour
{
    // GameManagerへの参照を保持する変数
    private TutorialManager TutoManager;
    // M5StickReaderへの参照を保持する変数
    private M5StickReader m5StickReader;

    [SerializeField]
    public AudioClip[] se;
    [SerializeField]
    AudioSource audioSource;


    // StartメソッドでGameManagerを一度だけ探して保持しておく
    void Start()
    {
        // シーン内からGameManagerコンポーネントを探してくる
        TutoManager = FindFirstObjectByType<TutorialManager>();
        // シーン内からM5StickReaderコンポーネントを探してくる
        m5StickReader = M5StickReader.Instance;

        if (m5StickReader == null)
        {
            Debug.LogError("M5StickReaderが見つかりません。シーンに配置されていない可能性があります。");
        }
        m5StickReader.setPower(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*
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
        */

        if(m5StickReader.getTarget_y() < -3.0f)
        {
            m5StickReader.setPushOKButton(true);
        }

        if (m5StickReader.getButtonFlag() && m5StickReader.getPushOKButton())
        {
            m5StickReader.setPushedButton(true);
        }

        if (m5StickReader.Consumepushedbutton() && !m5StickReader.getButtonFlag())
        {
            Vector2 worldPoint = m5StickReader.getThrowPos();
            m5StickReader.setPushedButton(false);
            confirmationtTargetHitSenser(worldPoint);
            m5StickReader.setThrowedActionFlag(true);
            m5StickReader.setPushOKButton(false);
        }

        Debug.Log("x: " + m5StickReader.getThrowPos().x + " y: " + m5StickReader.getThrowPos().y + " throw: " + m5StickReader.getThrowedActionFlag());
    }

    private void confirmationtTargetHitSenser(Vector2 worldPoint)
    {
        float hitRadius = 0.8f; // 半径は調整可能
        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPoint, hitRadius);
        foreach (Collider2D col in hits)
        {
            Target target = col.GetComponent<Target>();
            if (target != null)
            {
                if (TutoManager != null)
                {
                    TutoManager.AddScore(target.points);
                    if (target.points > 0)
                    {
                        TutoManager.AddCombo(1);
                    }
                    else
                    {
                        TutoManager.ResetComboAndHalveGauge();
                    }
                }
                target.Hit();
                audioSource.PlayOneShot(se[Random.Range(0, se.Length)]);
            }
        }
        m5StickReader.setPower(0.0f);

    }
}