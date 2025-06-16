// Target.cs

using UnityEngine;

public class Target : MonoBehaviour
{
    // このオブジェクトがマウスでクリックされた時に自動的に呼び出されるメソッド
    private void OnMouseDown()
    {
        // 動作確認のため、コンソールにメッセージを表示
        Debug.Log("的「" + gameObject.name + "」がクリックされました！");

        // このゲームオブジェクト（自分自身）をシーンから削除する
        Destroy(gameObject);
    }
}
