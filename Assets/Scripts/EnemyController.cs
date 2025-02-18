using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public float attackDamage;   // 공격력
    public float attackRange;    // 공격 범위
    public float moveSpeed;      // 이동 속도
    public float attackCooldown; // 공격 쿨타임
    public float maxHealth;      // 최대 체력
    private float currentHealth; // 현재 체력
    public float knockbackForce; // 넉백(밀림) 힘
    public Text healthText;      // 체력 UI 텍스트

    [Header("Targeting Settings")]
    public List<string> attackPriorityTags; // 공격 우선순위 태그 목록

    private Transform target;           // 현재 공격 대상
    private float lastAttackTime;       // 마지막 공격 시간
    private bool knockbackApplied = false;  // 넉백이 적용되었는지 여부 (한 번만 적용)
    private bool isDisabled = false;        // 이동 및 행동이 정지된 상태 여부

    private ResourceManager resourceManager; // ResourceManager 참조

    void Start()
    {
        currentHealth = maxHealth; // 체력 초기화
        resourceManager = FindObjectOfType<ResourceManager>(); // ResourceManager 검색
        UpdateHealthText(); // 체력 UI 갱신
    }

    void Update()
    {
        if (isDisabled) return; // 정지 상태이면 Update 실행 중지

        FindClosestTarget(); // 가장 가까운 대상 찾기

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget(); // 대상이 공격 범위 밖이면 이동
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack(target.gameObject); // 공격 실행
            }
        }
    }

    // 피해를 입는 메서드
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // 체력이 최대 체력의 30% 이하로 떨어지면 넉백 적용 (한 번만 적용)
        if (currentHealth <= maxHealth * 0.3f && !knockbackApplied)
        {
            // x축 방향으로 넉백 적용 (기본적으로 오른쪽 방향)
            ApplyKnockback(new Vector2(1, 0), knockbackForce);
            knockbackApplied = true;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("적 체력: " + currentHealth);
        UpdateHealthText(); // 체력 UI 갱신
    }

    // 사망 처리 메서드
    private void Die()
    {
        Debug.Log("적 사망");
        if (resourceManager != null)
        {
            resourceManager.OnEnemyDefeated(gameObject); // 적 처치 보상 처리
        }
        Destroy(gameObject);
    }

    // 체력 UI 갱신 메서드
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0");
        }
        else
        {
            Debug.LogWarning("healthText가 할당되지 않았습니다.");
        }
    }

    // 가장 가까운 대상을 찾는 메서드 (우선순위 태그 순으로 탐색)
    void FindClosestTarget()
    {
        target = null;
        float closestDistance = Mathf.Infinity;

        // 우선순위 태그 목록에 따라 대상 탐색
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
            if (target != null)
            {
                return; // 우선순위 태그에서 하나라도 대상이 발견되면 종료
            }
        }
    }

    // 대상 방향으로 이동하는 메서드
    void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    // 대상에게 공격을 수행하는 메서드
    void Attack(GameObject targetObject)
    {
        Debug.Log("적이 공격합니다: " + targetObject.name);

        // PlayerController가 있을 경우 플레이어에 피해 적용
        PlayerController playerController = targetObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage);
        }

        // base_player가 있을 경우 기지에 피해 적용
        base_player playerBase = targetObject.GetComponent<base_player>();
        if (playerBase != null)
        {
            playerBase.TakeDamage((int)attackDamage);
        }
    }

    // 넉백 효과를 적용하는 메서드
    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        // 현재 코드는 x축 오른쪽 방향으로만 넉백을 적용합니다.
        Vector2 fixedKnockbackDirection = new Vector2(1, 0).normalized;
        transform.position += (Vector3)(fixedKnockbackDirection * knockbackForce * Time.deltaTime);
    }

    // 지정 시간 동안 적을 정지시키는 메서드 (Disable)
    public void DisableEnemy(float duration)
    {
        if (!isDisabled)
        {
            isDisabled = true;
            StartCoroutine(EnableEnemyAfterDelay(duration));
        }
    }

    // 일정 시간 후 적의 정지 상태를 해제하는 코루틴
    private IEnumerator EnableEnemyAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDisabled = false; // 정지 상태 해제
    }

    // 에디터 상에서 공격 범위를 시각적으로 표시하는 메서드
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
