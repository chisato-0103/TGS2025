using UnityEngine;

public class RetelikuruImage : MonoBehaviour
{
    private M5StickReader m5StickReader;
    [SerializeField] private GameObject Rete;

    void Start()
    {
        // シーン内からM5StickReaderを取得
        m5StickReader = FindFirstObjectByType<M5StickReader>();
        if (m5StickReader == null)
        {
            Debug.LogError("M5StickReaderが見つかりません！");
        }


        if (Rete != null)
        {
            //Rete.SetActive(false); // 初期状態は非表示
        }
    }

    void Update()
    {
        if (m5StickReader == null || Rete == null) return;

        bool isPressed = m5StickReader.getButtonFlag() && m5StickReader.getPushOKButton();
        //Rete.SetActive(isPressed);

        if (m5StickReader != null && Rete != null)
        {
            // M5Stickの座標をScene上のSpriteに反映
            float x = m5StickReader.getTarget_x();
            float y = m5StickReader.getTarget_y();

            if(isPressed)
                Rete.transform.position = new Vector3(x, y, Rete.transform.position.z);// Z座標はそのまま維持
            else
                Rete.transform.position = new Vector3(-99, -99, Rete.transform.position.z);// Z座標はそのまま維持

        }
    }
}