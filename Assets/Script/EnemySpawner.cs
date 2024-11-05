using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 인스펙터에서 여러 적 유닛 프리팹을 할당할 수 있는 리스트
    public List<GameObject> enemyPrefabs;

    // 생성 간격을 조정할 수 있는 변수
    public float spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true) // 무한 반복
        {
            // 리스트에서 랜덤으로 프리팹 선택
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // 적 유닛을 x = -6 위치에 생성
            Vector3 spawnPosition = new Vector3(6f, transform.position.y, transform.position.z);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // 지정한 간격만큼 대기 후 다음 생성으로 이동
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
