using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void change_button()
    {
        ScreenManager screenManager = FindObjectOfType<ScreenManager>();
        if (screenManager != null)
        {
            screenManager.GoToRankingScene();
        }
        else
        {
            Debug.LogError("ScreenManagerが見つかりませんでした");
        }
    }
}
