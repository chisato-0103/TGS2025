// Target.cs

using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{
    //この的の点数（Inspectorから変更可能）
    [SerializeField]
    public int points = 10;

    //この的が自動で消えるまでの時間（秒）
    [SerializeField]
    private float lifetime = 3.0f;

    //ヒット時に表示するエフェクト用スプライト
    [SerializeField]
    private Sprite hitEffectSprite;

    //現在のSpriteRendererへの参照
    private SpriteRenderer spriteRenderer;

    //オブジェクトが生成された時に一度だけ呼ばれる
    void Start()
    {
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // lifetime秒後に、このゲームオブジェクトを破棄する
        Destroy(gameObject, lifetime);
    }

    // このメソッドが外部から呼び出されたら、ヒットエフェクトを開始する
    public void Hit()
    {
        Debug.Log("的「" + gameObject.name + "」に命中！");
        StartHitEffect();
    }

    // ヒットエフェクトを開始するメソッド
    public void StartHitEffect()
    {
        // 自動削除タイマーをキャンセル（既にDestroyが予約されている場合）
        CancelInvoke();

        // ヒットエフェクトコルーチンを開始
        StartCoroutine(HitEffectCoroutine());
    }

    // ヒットエフェクトのコルーチン
    private IEnumerator HitEffectCoroutine()
    {
        // ヒットエフェクト用スプライトが設定されている場合は切り替える
        if (hitEffectSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = hitEffectSprite;
            Debug.Log("ヒットエフェクト: スプライトを変更しました");
        }

        // 2秒待機
        yield return new WaitForSeconds(2.0f);

        // オブジェクトを破棄
        Debug.Log("ヒットエフェクト終了: オブジェクトを破棄");
        Destroy(gameObject);
    }
}
