using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float attackDamage = 50f; // 적의 공격 데미지
    public float attackRange = 1.5f;
    public float moveSpeed = 1.5f; // 이동 속도
    public float attackCooldown = 1f;
    public float maxHealth = 150f; // 최대 체력
    private float currentHealth;
    public List<GameObject> targets; // 공격 대상 리스트 (플레이어 및 기지 포함)

    public Text healthText; // 체력을 표시할 Text 컴포넌트

    private float lastAttackTime = 0f;
    private Transform target;

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

            // 공격 범위에 들어오지 않았을 때 대상에게 이동
            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                // 공격 범위 내에 있고, 공격 대기 시간이 지났을 때 공격
                lastAttackTime = Time.time;
                Attack(target.gameObject);
            }
        }
    }

    // 적이 데미지를 받았을 때 호출되는 메서드
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("적 체력: " + currentHealth); // 체력 감소 메시지 출력
        UpdateHealthText(); // 체력 텍스트 업데이트
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

    // 적이 사망했을 때 호출되는 메서드
    private void Die()
    {
        Debug.Log("적 사망");
        Destroy(gameObject);
    }

    // 리스트에서 가장 가까운 플레이어 또는 기지를 찾기
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

    // 타겟에게 이동
    void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    // 타겟을 공격
    void Attack(GameObject targetObject)
    {
        Debug.Log("적이 공격을 수행합니다: " + targetObject.name);

        // 대상이 기지인지 확인하고 데미지 적용
        base_player basePlayer = targetObject.GetComponent<base_player>();
        if (basePlayer != null)
        {
            basePlayer.TakeDamage((int)attackDamage); // 기지에 데미지
            return;
        }

        // 대상이 플레이어인지 확인하고 데미지 적용
        PlayerController playerController = targetObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage); // 플레이어에 데미지
        }
    }

    // 선택된 상태에서만 공격 범위를 시각적으로 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
