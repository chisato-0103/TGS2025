// PlayerInput.cs

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    void Update()
    {
        // マウスの左ボタンがクリックされた瞬間を検知
        if (Input.GetMouseButtonDown(0))
        {
            // マウスカーソルの位置をカメラから見たゲーム世界の座標に変換
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // その座標から見えない光線（レイ）を飛ばし、何かに当たったか調べる
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            // もし何かのColliderに当たっていたら
            if (hit.collider != null)
            {
                // 当たったオブジェクトから "Target" スクリプトを探す
                Target target = hit.collider.GetComponent<Target>();

                // もし "Target" スクリプトが見つかったら（＝当たったのが的だったら）
                if (target != null)
                {
                    // その的の Hit() メソッドを呼び出す
                    target.Hit();
                }
            }
        }
    }
}
