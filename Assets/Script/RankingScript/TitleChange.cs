using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleChange : MonoBehaviour
{
    public void change_button()
    {
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
