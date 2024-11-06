using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class TimedEnemy
    {
        public GameObject enemyPrefab;   // 등장할 유닛 프리팹
        public float spawnTimeInSeconds; // 유닛 등장 시간 (초 단위)
    }

    public List<TimedEnemy> timedEnemies = new List<TimedEnemy>(); // 등장할 유닛 리스트
    private Vector3 spawnPosition; // 고정된 유닛 등장 위치 설정

    private void Start()
    {
        // 유닛의 스폰 위치 설정
        spawnPosition = new Vector3(6f, -1.4f, transform.position.z);

        // 각 유닛에 대해 지정된 시간에 생성하도록 코루틴 시작
        foreach (TimedEnemy timedEnemy in timedEnemies)
        {
            StartCoroutine(SpawnTimedEnemy(timedEnemy));
        }
    }

    private IEnumerator SpawnTimedEnemy(TimedEnemy timedEnemy)
    {
        // 인스펙터에서 설정한 시간만큼 대기 후 유닛 생성
        yield return new WaitForSeconds(timedEnemy.spawnTimeInSeconds);

        if (timedEnemy.enemyPrefab != null)
        {
            Instantiate(timedEnemy.enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
