using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック（またはスマホのタップ）
        {
            Debug.Log("画面がクリックされました");
            ScreenManager screenManager = FindObjectOfType<ScreenManager>();
            if (screenManager != null)
            {
                screenManager.GoToTitleScene();
            }
            else
            {
                Debug.LogError("ScreenManagerが見つかりませんでした");
            }
        }
    }
}
