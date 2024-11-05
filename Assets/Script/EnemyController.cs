using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float attackDamage; // 공격 데미지
    public float attackRange; // 공격 사거리
    public float moveSpeed; // 이동 속도
    public float attackCooldown; // 공격 대기 시간
    public float maxHealth; // 최대 체력
    private float currentHealth; // 현재 체력
    public float knockbackForce; // 넉백
    public Text healthText; // 체력을 표시할 Text 컴포넌트
    
    public List<GameObject> targets; // 공격 대상 리스트 (플레이어 및 기지 포함)
    private Transform target;

    private float lastAttackTime; // 마지막으로 공격한 시간을 저장
    private bool knockbackApplied = false; // 넉백 한 번만 적용되도록 설정

    void Start()
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 초기화
        UpdateHealthText(); // 초기 체력 표시
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

        Debug.Log("적 체력: " + currentHealth);
        UpdateHealthText();
    }

    void ApplyKnockback()
    {
        if (target != null)
        {
            Vector2 knockbackDirection = (transform.position - target.position).normalized;
            transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
        }
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

    private void Die()
    {
        Debug.Log("적 사망");
        Destroy(gameObject);
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
        Debug.Log("적이 공격을 수행합니다: " + targetObject.name);

        base_player basePlayer = targetObject.GetComponent<base_player>();
        if (basePlayer != null)
        {
            basePlayer.TakeDamage((int)attackDamage);
            return;
        }

        PlayerController playerController = targetObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
