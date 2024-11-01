using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackDamage = 20f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f; // 이동 속도
    public float attackCooldown = 1f;
    public List<GameObject> enemies; // 적 오브젝트 리스트 (기지 포함)

    private float lastAttackTime = 0f;
    private Transform targetEnemy;

    void Update()
    {
        FindClosestEnemy();

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);

            // 공격 범위에 들어오지 않았을 때 적에게 이동
            if (distanceToEnemy > attackRange)
            {
                MoveTowardsEnemy();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                // 공격 범위 내에 있고, 공격 대기 시간이 지났을 때 공격
                lastAttackTime = Time.time;
                Attack(targetEnemy.gameObject);
            }
        }

        DrawAttackRange(); // 공격 범위를 시각적으로 표시
    }

    // 리스트에서 가장 가까운 적 또는 기지를 찾기
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

    // 적에게 이동
    void MoveTowardsEnemy()
    {
        Vector2 direction = (targetEnemy.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetEnemy.position, moveSpeed * Time.deltaTime);
    }

    // 적 또는 기지를 공격
    void Attack(GameObject enemy)
    {
        Debug.Log("플레이어가 공격을 수행합니다: " + enemy.name);

        // 기지인지 확인하고 데미지 적용
        base_enemy baseEnemy = enemy.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage); // 기지에 데미지
            return;
        }

        // 적인지 확인하고 데미지 적용
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage); // 적에 데미지
        }
    }

    // 공격 범위를 시각적으로 표시
    void DrawAttackRange()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.right * attackRange, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.left * attackRange, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.up * attackRange, Color.red);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * attackRange, Color.red);
    }
} 