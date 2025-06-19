// PlayerInput.cs

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // [追加] GameManagerへの参照を保持する変数
    private GameManager gameManager;

    // [追加] StartメソッドでGameManagerを一度だけ探して保持しておく
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
                    // [追加] 的を破壊する直前に、スコアを加算する処理を呼び出す
                    // とりあえず10点加算する
                    if (gameManager != null)
                    {
                        gameManager.AddScore(10);
                    }

                    target.Hit();
                }
            }
        }
    }
}
