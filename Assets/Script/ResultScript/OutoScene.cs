using UnityEngine;
using UnityEngine.SceneManagement;

public class OutoScene : MonoBehaviour
{
    double timer = 0.0;
    [SerializeField] private double end = 8.0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= end)
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
        else
        {
            timer += Time.deltaTime;
        }
    }
}
