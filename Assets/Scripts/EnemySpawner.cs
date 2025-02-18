using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class TimedEnemy
    {
        public GameObject enemyPrefab;   // 적 프리팹
        public float spawnInterval;      // 적 스폰 간격 (초 단위)
    }

    public List<TimedEnemy> timedEnemies = new List<TimedEnemy>(); // 시간별 적 목록
    private Vector3 spawnPosition; // 적이 스폰될 위치

    private void Start()
    {
        // 적이 스폰될 위치를 설정 (x=6, y=-1.4, z는 현재 오브젝트의 z값 사용)
        spawnPosition = new Vector3(6f, -1.4f, transform.position.z);

        // 각 TimedEnemy마다 코루틴을 시작하여 주기적으로 적을 스폰합니다.
        foreach (TimedEnemy timedEnemy in timedEnemies)
        {
            StartCoroutine(SpawnTimedEnemy(timedEnemy));
        }
    }

    // 주어진 TimedEnemy 정보를 기반으로 적을 주기적으로 스폰하는 코루틴
    private IEnumerator SpawnTimedEnemy(TimedEnemy timedEnemy)
    {
        while (true) // 무한 반복하여 계속 스폰
        {
            // 지정된 간격만큼 대기 후 스폰
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
