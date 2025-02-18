using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float attackDamage; // 플레이어 공격력
    public float attackRange; // 공격 범위
    public float moveSpeed; // 이동 속도
    public float attackCooldown; // 공격 쿨다운 (공격 간격)
    public float maxHealth; // 최대 체력
    private float currentHealth; // 현재 체력
    public float knockbackForce; // 넉백(밀려나는) 힘
    public Text healthText; // 체력을 표시할 UI 텍스트

    [Header("Targeting Settings")]
    public List<string> attackPriorityTags; // 공격 우선순위 태그 리스트

    private Transform target; // 현재 공격할 대상
    private float lastAttackTime; // 마지막 공격 시간을 기록
    private bool knockbackApplied = false; // 넉백이 한 번만 적용되도록 체크

    void Start()
    {
        currentHealth = maxHealth; // 체력을 최대치로 설정
        UpdateHealthText(); // UI 업데이트
    }

    void Update()
    {
        FindClosestTarget(); // 가장 가까운 적을 찾음

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // 적과의 거리가 공격 범위보다 크면 이동
            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget();
            }
            // 공격 범위 내에 있고, 쿨다운이 끝났다면 공격
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack(target.gameObject);
            }
        }
    }

    // 플레이어가 피해를 입는 메서드
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // 체력이 최대 체력의 30% 이하로 떨어지면 한 번만 넉백 적용
        if (currentHealth <= maxHealth * 0.3f && !knockbackApplied)
        {
            ApplyKnockback();
            knockbackApplied = true; // 넉백이 한 번만 실행되도록 설정
        }

        // 체력이 0 이하가 되면 사망 처리
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("플레이어 체력: " + currentHealth);
        UpdateHealthText();
    }

    // 플레이어 사망 처리
    private void Die()
    {
        Debug.Log("플레이어 사망");
        Destroy(gameObject); // 플레이어 오브젝트 삭제
    }

    // 체력 UI 업데이트
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0"); // 체력을 소수점 없이 표시
        }
        else
        {
            Debug.LogWarning("healthText가 설정되지 않았습니다."); // healthText가 설정되지 않았을 경우 경고 출력
        }
    }

    // 가장 가까운 적을 찾는 메서드 (우선순위 태그 기반 탐색)
    void FindClosestTarget()
    {
        target = null;
        float closestDistance = Mathf.Infinity; // 초기값을 무한대로 설정

        // 공격 우선순위 태그 순서대로 적 탐색
        foreach (string priorityTag in attackPriorityTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(priorityTag); // 태그에 해당하는 적 검색
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = enemy.transform; // 가장 가까운 적을 타겟으로 설정
                }
            }
            if (target != null)
            {
                return; // 가장 가까운 적을 찾으면 즉시 종료
            }
        }
    }

    // 적을 향해 이동하는 메서드
    void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized; // 이동 방향 계산
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime); // 이동
    }

    // 적을 공격하는 메서드
    void Attack(GameObject targetObject)
    {
        Debug.Log("플레이어가 적을 공격합니다: " + targetObject.name);

        // base_enemy 스크립트를 가진 적을 공격
        base_enemy baseEnemy = targetObject.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage);
            return;
        }

        // EnemyController 스크립트를 가진 적을 공격
        EnemyController enemyController = targetObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage);
        }
    }

    // 플레이어가 넉백되는 메서드 (체력이 일정 이하일 때 발동)
    void ApplyKnockback()
    {
        // 넉백 방향을 왼쪽(x축 기준)으로 설정
        Vector2 knockbackDirection = Vector2.left;
        transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
        Debug.Log("플레이어가 넉백 효과를 받음");
    }

    // 공격 범위를 기즈모(Gizmos)로 시각적으로 표시하는 메서드 (에디터에서 확인 가능)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
