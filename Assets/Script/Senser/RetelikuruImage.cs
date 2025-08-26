using UnityEngine;

public class RetelikuruImage : MonoBehaviour
{
    private M5StickReader m5StickReader;
    public Transform rete; // RectTransform ではなく Transform に変更

    void Start()
    {
        // シーン内からM5StickReaderを取得
        m5StickReader = FindFirstObjectByType<M5StickReader>();
        if (m5StickReader == null)
        {
            Debug.LogError("M5StickReaderが見つかりません！");
        }
    }

    void Update()
    {
        if (m5StickReader != null && rete != null)
        {
            // M5Stickの座標をScene上のSpriteに反映
            float x = m5StickReader.getTarget_x();
            float y = m5StickReader.getTarget_y();

            rete.position = new Vector3(x, y, rete.position.z); // Z座標はそのまま維持
        }
    }
}
