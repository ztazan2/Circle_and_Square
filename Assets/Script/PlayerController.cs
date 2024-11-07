using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float attackDamage; // 공격 데미지
    public float attackRange; // 공격 사거리
    public float moveSpeed; // 이동 속도
    public float attackCooldown; // 공격 대기 시간
    public float maxHealth; // 최대 체력
    private float currentHealth; // 현재 체력
    public float knockbackForce; // 넉백 힘
    public Text healthText; // 체력을 표시할 Text 컴포넌트

    [Header("Targeting Settings")]
    public List<string> targetTags; // 추적할 태그 리스트
    public List<string> attackPriorityTags; // 공격 우선순위 태그 리스트

    private Transform target; // 현재 공격 대상의 Transform
    private float lastAttackTime; // 마지막 공격 시간을 저장
    private bool knockbackApplied = false; // 넉백 한 번만 적용되도록 설정

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    void Update()
    {
        FindClosestTarget();

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack(target.gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= maxHealth * 0.3f && !knockbackApplied)
        {
            ApplyKnockback();
            knockbackApplied = true; // 넉백이 한 번만 적용되도록 설정
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("플레이어 체력: " + currentHealth);
        UpdateHealthText();
    }

    private void Die()
    {
        Debug.Log("플레이어 사망");
        Destroy(gameObject);
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0");
        }
        else
        {
            Debug.LogWarning("healthText가 연결되지 않았습니다.");
        }
    }

    void FindClosestTarget()
    {
        target = null;
        float closestDistance = Mathf.Infinity;

        // 우선순위에 따른 대상 탐색
        foreach (string priorityTag in attackPriorityTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(priorityTag);
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = enemy.transform;
                }
            }
            // 우선순위 태그에 대상이 있으면 찾기 종료
            if (target != null)
            {
                return;
            }
        }

        // 우선순위 태그에서 대상을 찾지 못했을 경우 일반 대상 태그에서 찾기
        foreach (string tag in targetTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = enemy.transform;
                }
            }
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void Attack(GameObject targetObject)
    {
        Debug.Log("플레이어가 공격을 수행합니다: " + targetObject.name);

        base_enemy baseEnemy = targetObject.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage);
            return;
        }

        EnemyController enemyController = targetObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage);
        }
    }

    void ApplyKnockback()
    {
        if (target != null)
        {
            // 넉백 방향을 좌측으로만 제한
            Vector2 knockbackDirection = Vector2.left;
            transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
            Debug.Log("좌측 방향으로 넉백 적용됨");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
