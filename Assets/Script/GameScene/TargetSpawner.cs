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


    void Start()
    {
        // 開始するコルーチンは変更なし
        StartCoroutine(SpawnTargets());
    }

    private IEnumerator SpawnTargets()
    {
        while (true)
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

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
