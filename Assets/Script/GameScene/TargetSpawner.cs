// TargetSpawner.cs

using System.Collections;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    // 単一のGameObjectから、GameObjectの配列[]に変更
    [SerializeField]
    private GameObject[] targetPrefabs; // 生成する的のプレハブ（複数形に）

    [SerializeField]
    private float spawnInterval = 1.5f;

    [SerializeField]
    private float spawnAreaWidth = 8f;

    [SerializeField]
    private float spawnAreaHeight = 4f;
    
    private bool isFeverMode = false; // フィーバーモードフラグ
    private bool isVideoPlaying = false; // フィーバー動画再生中フラグ
    
    // --- フィーバータイム関連の変数 ---
    [SerializeField]
    private float femaleGorillaSpawnInterval = 0.3f; // female-gorilla生成間隔
    
    private GameObject femaleGorillaPrefab; // female-gorillaプレファブ（targetPrefabsから検索）
    private Coroutine feverSpawnCoroutine; // female-gorilla生成コルーチン


    void Start()
    {
        // targetPrefabs配列からfemale-gorillaを検索
        FindFemaleGorillaPrefab();
        
        // 開始するコルーチンは変更なし
        StartCoroutine(SpawnTargets());
    }

    private IEnumerator SpawnTargets()
    {
        while (true)
        {
            // フィーバーモード中または動画再生中はスポーンを停止
            if (!isFeverMode && !isVideoPlaying)
            {
                // --- スポーンするプレハブをランダムに選択 ---
                // 0から、登録されたプレハブの数-1までの間で、ランダムな整数を一つ選ぶ
                int randomIndex = Random.Range(0, targetPrefabs.Length);

                // 選ばれたランダムな番号を使って、配列からプレハブを一つ取り出す
                GameObject prefabToSpawn = targetPrefabs[randomIndex];
                // --------------------------------------------------

                float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                float randomY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
                Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

                // 先ほどランダムに選んだプレハブ(prefabToSpawn)を生成する
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    // フィーバーモード開始（動画再生開始）
    public void StartFeverMode()
    {
        isFeverMode = true;
        isVideoPlaying = true; // 動画再生開始
        Debug.Log("TargetSpawner フィーバーモード開始 - 動画再生中はスポーン停止");
    }
    
    // フィーバー動画終了後のfemale-gorilla生成開始
    public void StartFemaleGorillaSpawn()
    {
        isVideoPlaying = false; // 動画終了
        Debug.Log("TargetSpawner 動画終了 - female-gorilla生成開始");
        
        // female-gorilla連続生成開始
        if (femaleGorillaPrefab != null)
        {
            feverSpawnCoroutine = StartCoroutine(SpawnFemaleGorillaDuringFever());
        }
        else
        {
            Debug.LogWarning("female-gorillaプレファブがtargetPrefabs配列に見つかりません");
        }
    }
    
    // スポーンを停止（透過動画用）
    public void StopSpawning()
    {
        isVideoPlaying = true;
        Debug.Log("TargetSpawner スポーン停止");
    }
    
    // スポーンを再開（透過動画終了用）
    public void ResumeSpawning()
    {
        isVideoPlaying = false;
        Debug.Log("TargetSpawner スポーン再開");
    }
    
    // フィーバーモード終了
    public void EndFeverMode()
    {
        isFeverMode = false;
        isVideoPlaying = false;
        Debug.Log("TargetSpawner フィーバーモード終了");
        
        // female-gorilla生成を停止
        if (feverSpawnCoroutine != null)
        {
            StopCoroutine(feverSpawnCoroutine);
            feverSpawnCoroutine = null;
        }
        
        // 全スポーンオブジェクトをデストロイ
        DestroyAllSpawnedObjects();
    }
    
    // フィーバータイム中のfemale-gorilla連続生成
    private IEnumerator SpawnFemaleGorillaDuringFever()
    {
        while (isFeverMode)
        {
            // ランダムな位置に生成（スポーンエリアと同じ範囲）
            float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
            float randomY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
            
            Instantiate(femaleGorillaPrefab, spawnPosition, Quaternion.identity);
            
            yield return new WaitForSeconds(femaleGorillaSpawnInterval);
        }
    }
    
    // targetPrefabs配列からfemale-gorillaプレファブを検索
    private void FindFemaleGorillaPrefab()
    {
        foreach (GameObject prefab in targetPrefabs)
        {
            if (prefab != null && prefab.name.Contains("female-gorilla"))
            {
                femaleGorillaPrefab = prefab;
                Debug.Log("female-gorillaプレファブを発見: " + prefab.name);
                return;
            }
        }
        Debug.LogWarning("targetPrefabs配列に'female-gorilla'を含む名前のプレファブが見つかりません");
    }
    
    // 全スポーンオブジェクトをデストロイ
    private void DestroyAllSpawnedObjects()
    {
        // プレファブ名に基づいてオブジェクトを検索・削除
        foreach (GameObject prefab in targetPrefabs)
        {
            if (prefab != null)
            {
                // プレファブ名（Clone）付きでシーン内のオブジェクトを検索
                string objectName = prefab.name + "(Clone)";
                GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("Untagged");
                
                foreach (GameObject obj in spawnedObjects)
                {
                    if (obj.name == objectName || obj.name.StartsWith(prefab.name))
                    {
                        Debug.Log("フィーバー終了 - デストロイ: " + obj.name);
                        Destroy(obj);
                    }
                }
            }
        }
        
        // より確実な方法：プレファブ名を含むオブジェクトをすべて削除
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("gorilla"))
            {
                Debug.Log("フィーバー終了 - ゴリラオブジェクトをデストロイ: " + obj.name);
                Destroy(obj);
            }
        }
    }
}
