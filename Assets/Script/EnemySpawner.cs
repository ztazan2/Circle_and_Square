using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 기본 랜덤 유닛 리스트와 생성 간격
    public List<GameObject> enemyPrefabs;
    public float spawnInterval;

    // 특정 시간에 등장할 유닛 설정
    [System.Serializable]
    public class TimedEnemy
    {
        public GameObject enemyPrefab;   // 등장할 유닛 프리팹
        public float spawnTimeInSeconds; // 등장 시간 (초 단위), 인스펙터에서 설정 가능
    }

    public TimedEnemy timedEnemyA; // A 유닛
    public TimedEnemy timedEnemyB; // B 유닛

    // 고정된 유닛 등장 위치 설정
    private Vector3 spawnPosition;

    private void Start()
    {
        // 일반 유닛의 스폰 위치 설정 (x: 6, y:-1.5로 고정)
        spawnPosition = new Vector3(6f, -1.4f, transform.position.z);

        // 코루틴 시작
        StartCoroutine(SpawnEnemies());

        // A와 B 유닛의 지정된 위치에서 생성
        StartCoroutine(SpawnTimedEnemy(timedEnemyA, new Vector3(6f, -1.2f, 0f))); // A 유닛 위치
        StartCoroutine(SpawnTimedEnemy(timedEnemyB, new Vector3(6f, -1.2f, 0f))); // B 유닛 위치
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // 랜덤으로 프리팹 선택 및 생성
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // x 좌표가 6, y 좌표는 현재 스크립트가 붙은 오브젝트의 y 위치
            Vector3 spawnPositionWithDynamicY = new Vector3(6f, transform.position.y, transform.position.z);

            // 지정된 위치에 적 유닛 생성
            Instantiate(enemyPrefab, spawnPositionWithDynamicY, Quaternion.identity);

            // 지정한 간격만큼 대기 후 다음 생성으로 이동
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnTimedEnemy(TimedEnemy timedEnemy, Vector3 position)
    {
        // 인스펙터에서 설정한 시간만큼 대기 후 유닛 생성
        yield return new WaitForSeconds(timedEnemy.spawnTimeInSeconds);

        if (timedEnemy.enemyPrefab != null)
        {
            Instantiate(timedEnemy.enemyPrefab, position, Quaternion.identity);
        }
    }
}
