using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public List<GameObject> targets; // 공격 대상 리스트 (적 및 기지 포함)
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
        float closestDistance = Mathf.Infinity;
        target = null;

        foreach (GameObject obj in targets)
        {
            if (obj != null)
            {
                float distance = Vector2.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = obj.transform;
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
            Vector2 knockbackDirection = (transform.position - target.position).normalized;
            transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
            Debug.Log("넉백 적용됨");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!targets.Contains(enemy))
        {
            targets.Add(enemy); // 새로운 적을 공격 대상 리스트에 추가
            Debug.Log("새로운 적이 등록되었습니다: " + enemy.name);
        }
    }
}
