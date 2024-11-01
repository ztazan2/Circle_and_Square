using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackDamage = 100f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f; // 이동 속도
    public float attackCooldown = 1f;
    public float maxHealth = 100f; // 최대 체력
    private float currentHealth;
    public List<GameObject> enemies; // 적 오브젝트 리스트 (기지 포함)

    public Text healthText; // 체력을 표시할 Text 컴포넌트

    private float lastAttackTime = 0f;
    private Transform targetEnemy;

    void Start()
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 초기화
        UpdateHealthText(); // 초기 체력 표시
    }

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
    }

    // 플레이어가 데미지를 받았을 때 호출되는 메서드
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("플레이어 체력: " + currentHealth); // 체력 감소 메시지 출력
        UpdateHealthText(); // 체력 텍스트 업데이트
    }

    // 플레이어가 사망했을 때 호출되는 메서드
    private void Die()
    {
        Debug.Log("플레이어 사망");
        Destroy(gameObject);
    }

    // 체력 텍스트를 업데이트하는 메서드
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0"); // 소수점 없이 표시
        }
        else
        {
            Debug.LogWarning("healthText가 연결되지 않았습니다.");
        }
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

    // 선택된 상태에서만 공격 범위를 시각적으로 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
