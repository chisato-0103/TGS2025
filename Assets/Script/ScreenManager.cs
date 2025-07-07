using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

// 画面の切り替えを管理するクラス
// スタート画面、ゲーム画面、結果画面などを切り替える
public class ScreenManager: MonoBehaviour
{
    // どこからでもScreenManagerを使えるようにするための変数
    // これがあると、他のスクリプトから「ScreenManager.Instance」で呼び出せる
    public static ScreenManager Instance;

    // ゲームが始まったときに最初に呼ばれる関数
    private void Awake()
    {
        // ScreenManagerが1つだけ存在するようにする（重複を防ぐ）
        if (Instance == null)
        {
            Instance = this; // 自分をInstanceに登録
            DontDestroyOnLoad(gameObject); // 画面が変わっても消えないようにする
        }
        else
        {
            Destroy(gameObject); // すでに存在する場合は、重複した方を削除
        }
    }

    // 毎フレーム（1秒間に60回くらい）呼ばれる関数
    private void Update()
    {
        // スペースキーが押されたらゲームを開始する
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame(); // ゲーム開始の関数を呼び出す
        }
    }

    // ゲームを開始する関数
    public void StartGame()
    {
        Debug.Log("ゲームスタート！"); // コンソールに「ゲームスタート！」と表示

        SceneManager.LoadScene("GameScene"); // "GameScene" という名前の画面に切り替える
    }

    // 結果画面に切り替える関数
    public void LoadResultScene()
    {
        Debug.Log("リザルトシーンに移動！"); // コンソールに「リザルトシーンに移動！」と表示
        SceneManager.LoadScene("ResultScene"); // "ResultScene" という名前の画面に切り替える
    }
}
