using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �ν����Ϳ��� ���� �� ���� �������� �Ҵ��� �� �ִ� ����Ʈ
    public List<GameObject> enemyPrefabs;

    // ���� ������ ������ �� �ִ� ����
    public float spawnInterval;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true) // ���� �ݺ�
        {
            // ����Ʈ���� �������� ������ ����
            int randomIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject enemyPrefab = enemyPrefabs[randomIndex];

            // �� ������ x = -6 ��ġ�� ����
            Vector3 spawnPosition = new Vector3(6f, transform.position.y, transform.position.z);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // ������ ���ݸ�ŭ ��� �� ���� �������� �̵�
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
