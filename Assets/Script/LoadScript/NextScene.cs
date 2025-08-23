using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float start = 0;
    [SerializeField] private float end = 4;
    bool flag = false;
    private ScreenManager screenManager;
    void Start()
    {
        screenManager = FindObjectOfType<ScreenManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start >= end && flag == false)
        {
            screenManager.StartGame();
            flag = true;
        }
        else
        {
            start += Time.deltaTime;
        }
    }
}
