// Target.cs

using UnityEngine;

public class Target : MonoBehaviour
{
    // このメソッドが外部から呼び出されたら、オブジェクトを破壊する
    public void Hit()
    {
        Debug.Log("的「" + gameObject.name + "」に命中！");
        Destroy(gameObject);
    }
}
