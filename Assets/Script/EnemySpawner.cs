using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �ν����Ϳ��� ���� �� ���� �������� �Ҵ��� �� �ִ� ����Ʈ
    public List<GameObject> enemyPrefabs;

    // ���� ������ ������ �� �ִ� ����
    public float spawnInterval;

    // PlayerController ����
    private PlayerController playerController;

    private void Start()
    {
        // PlayerController�� ã�� �ڷ�ƾ ����
        playerController = FindObjectOfType<PlayerController>();
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
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // ������ ���� PlayerController�� enemies ����Ʈ�� �߰�
            if (playerController != null)
            {
                playerController.RegisterEnemy(newEnemy);
            }

            // ������ ���ݸ�ŭ ��� �� ���� �������� �̵�
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
