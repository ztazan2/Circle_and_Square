using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �⺻ ���� ���� ����Ʈ�� ���� ����
    public List<GameObject> enemyPrefabs;
    public float spawnInterval;

    // Ư�� �ð��� ������ ���� ����
    [System.Serializable]
    public class TimedEnemy
    {
        public GameObject enemyPrefab;   // ������ ���� ������
        public float spawnTimeInSeconds; // ���� �ð� (�� ����), �ν����Ϳ��� ���� ����
    }

    public TimedEnemy timedEnemyA; // A ����
    public TimedEnemy timedEnemyB; // B ����

    // ������ ���� ���� ��ġ ����
    private Vector3 spawnPosition;

    private void Start()
    {
        // �Ϲ� ������ ���� ��ġ ���� (x: 6, y:-1.5�� ����)
        spawnPosition = new Vector3(6f, -1.4f, transform.position.z);

        // �ڷ�ƾ ����
        StartCoroutine(SpawnEnemies());

        // A�� B ������ ������ ��ġ���� ����
        StartCoroutine(SpawnTimedEnemy(timedEnemyA, new Vector3(6f, -1.2f, 0f))); // A ���� ��ġ
        StartCoroutine(SpawnTimedEnemy(timedEnemyB, new Vector3(6f, -1.2f, 0f))); // B ���� ��ġ
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // �������� ������ ���� �� ����
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // x ��ǥ�� 6, y ��ǥ�� ���� ��ũ��Ʈ�� ���� ������Ʈ�� y ��ġ
            Vector3 spawnPositionWithDynamicY = new Vector3(6f, transform.position.y, transform.position.z);

            // ������ ��ġ�� �� ���� ����
            Instantiate(enemyPrefab, spawnPositionWithDynamicY, Quaternion.identity);

            // ������ ���ݸ�ŭ ��� �� ���� �������� �̵�
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnTimedEnemy(TimedEnemy timedEnemy, Vector3 position)
    {
        // �ν����Ϳ��� ������ �ð���ŭ ��� �� ���� ����
        yield return new WaitForSeconds(timedEnemy.spawnTimeInSeconds);

        if (timedEnemy.enemyPrefab != null)
        {
            Instantiate(timedEnemy.enemyPrefab, position, Quaternion.identity);
        }
    }
}
