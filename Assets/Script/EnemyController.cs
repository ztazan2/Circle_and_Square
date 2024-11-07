using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
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

    private Transform target; // 공격 대상의 Transform
    private float lastAttackTime; // 마지막 공격 시간을 저장
    private bool knockbackApplied = false; // 넉백 한 번만 적용되도록 설정

    private ResourceManager resourceManager; // ResourceManager 참조

    void Start()
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 초기화
        resourceManager = FindObjectOfType<ResourceManager>(); // ResourceManager 검색
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
            // 기본 넉백을 적용하기 위해 ApplyKnockback 호출
            ApplyKnockback(Vector2.right, knockbackForce); // 항상 오른쪽으로만 밀리도록 수정
            knockbackApplied = true; // 넉백이 한 번만 적용되도록 설정
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("적 체력: " + currentHealth);
        UpdateHealthText(); // 체력 텍스트 업데이트
    }

    // 외부에서 넉백 방향과 힘을 받아 위치를 변경하는 메서드
    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        // 넉백 방향을 오른쪽(x축 양수 방향)으로 강제 설정
        knockbackDirection = Vector2.right;

        transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0"); // 체력 텍스트 업데이트
        }
        else
        {
            Debug.LogWarning("healthText가 연결되지 않았습니다.");
        }
    }

    private void Die()
    {
        Debug.Log("적 사망");

        // 적이 사망할 때 ResourceManager에 보상 호출
        if (resourceManager != null)
        {
            resourceManager.OnEnemyDefeated(gameObject);
        }

        Destroy(gameObject);
    }

    void FindClosestTarget()
    {
        target = null;
        float closestDistance = Mathf.Infinity;

        // 우선순위에 따른 대상 탐색
        foreach (string priorityTag in attackPriorityTags)
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(priorityTag);
            foreach (GameObject potentialTarget in potentialTargets)
            {
                float distance = Vector2.Distance(transform.position, potentialTarget.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = potentialTarget.transform;
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
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject potentialTarget in potentialTargets)
            {
                float distance = Vector2.Distance(transform.position, potentialTarget.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = potentialTarget.transform;
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
