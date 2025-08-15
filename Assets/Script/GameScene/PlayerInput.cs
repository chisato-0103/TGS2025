using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // GameManagerへの参照を保持する変数
    private GameManager gameManager;

    // StartメソッドでGameManagerを一度だけ探して保持しておく
    void Start()
    {
        // シーン内からGameManagerコンポーネントを探してくる
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

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
    }
}
