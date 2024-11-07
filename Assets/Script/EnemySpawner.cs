using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class TimedEnemy
    {
        public GameObject enemyPrefab;   // 등장할 유닛 프리팹
        public float spawnInterval;      // 유닛 등장 간격 (초 단위)
    }

    public List<TimedEnemy> timedEnemies = new List<TimedEnemy>(); // 등장할 유닛 리스트
    private Vector3 spawnPosition; // 고정된 유닛 등장 위치 설정

    private void Start()
    {
        // 유닛의 스폰 위치 설정
        spawnPosition = new Vector3(6f, -1.4f, transform.position.z);

        // 각 유닛에 대해 지정된 간격으로 생성하도록 코루틴 시작
        foreach (TimedEnemy timedEnemy in timedEnemies)
        {
            StartCoroutine(SpawnTimedEnemy(timedEnemy));
        }
    }

    private IEnumerator SpawnTimedEnemy(TimedEnemy timedEnemy)
    {
        while (true) // 지속적으로 적을 스폰하기 위해 무한 반복
        {
            // 설정한 간격만큼 대기 후 유닛 생성
            yield return new WaitForSeconds(timedEnemy.spawnInterval);

            if (timedEnemy.enemyPrefab != null)
            {
                Instantiate(timedEnemy.enemyPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Enemy prefab is missing.");
            }
        }
    }
}
