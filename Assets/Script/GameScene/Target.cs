// Target.cs

using UnityEngine;

public class Target : MonoBehaviour
{
    //この的の点数（Inspectorから変更可能）
    [SerializeField]
    public int points = 10;

    //この的が自動で消えるまでの時間（秒）
    [SerializeField]
    private float lifetime = 3.0f;

    //オブジェクトが生成された時に一度だけ呼ばれる
    void Start()
    {
        // lifetime秒後に、このゲームオブジェクトを破棄する
        Destroy(gameObject, lifetime);
    }

    // このメソッドが外部から呼び出されたら、オブジェクトを破壊する
    public void Hit()
    {
        Debug.Log("的「" + gameObject.name + "」に命中！");
        Destroy(gameObject);
    }
}
