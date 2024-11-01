using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackDamage = 20f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float attackCooldown = 1f;
    public List<GameObject> enemies; // �� ������Ʈ ����Ʈ (���� ����)

    private float lastAttackTime = 0f;
    private Transform targetEnemy;

    void Update()
    {
        FindClosestEnemy();

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);

            // ���� ������ ������ �ʾ��� �� ������ �̵�
            if (distanceToEnemy > attackRange)
            {
                MoveTowardsEnemy();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                // ���� ���� ���� �ְ�, ���� ��� �ð��� ������ �� ����
                lastAttackTime = Time.time;
                Attack(targetEnemy.gameObject);
            }
        }

        DrawAttackRange(); // ���� ������ �ð������� ǥ��
    }

    // ����Ʈ���� ���� ����� �� �Ǵ� ������ ã��
    void FindClosestEnemy()
    {
        float closestDistance = Mathf.Infinity;
        targetEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = enemy.transform;
                }
            }
        }
    }

    // ������ �̵�
    void MoveTowardsEnemy()
    {
        Vector2 direction = (targetEnemy.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetEnemy.position, moveSpeed * Time.deltaTime);
    }

    // �� �Ǵ� ������ ����
    void Attack(GameObject enemy)
    {
        Debug.Log("�÷��̾ ������ �����մϴ�: " + enemy.name);

        // �������� Ȯ���ϰ� ������ ����
        base_enemy baseEnemy = enemy.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage); // ������ ������
            return;
        }

        // ������ Ȯ���ϰ� ������ ����
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage); // ���� ������
        }
    }

    // ���� ������ �ð������� ǥ��
    void DrawAttackRange()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.right * attackRange, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.left * attackRange, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.up * attackRange, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * attackRange, Color.red);
    }
} 