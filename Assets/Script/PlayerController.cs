using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackDamage = 100f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f;
    public float attackCooldown = 1f;
    public float maxHealth = 100f;
    private float currentHealth;

    public List<GameObject> enemies = new List<GameObject>();
    private List<EnemyController> enemyControllers = new List<EnemyController>();

    public Text healthText;

    private float lastAttackTime = 0f;
    private GameObject targetEnemy;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();

        foreach (GameObject enemy in enemies)
        {
            RegisterEnemy(enemy);
        }
    }

    void Update()
    {
        FindClosestEnemy();

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.transform.position);

            if (distanceToEnemy > attackRange)
            {
                MoveTowardsEnemy();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack(targetEnemy);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

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
                    targetEnemy = enemy;
                }
            }
        }
    }

    void MoveTowardsEnemy()
    {
        Vector2 direction = (targetEnemy.transform.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetEnemy.transform.position, moveSpeed * Time.deltaTime);
    }

    void Attack(GameObject enemy)
    {
        Debug.Log("플레이어가 공격을 수행합니다: " + enemy.name);

        // 기지인지 확인하고 데미지 적용
        base_enemy baseEnemy = enemy.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage); // attackDamage를 int로 변환하여 기지에 데미지 적용
            return;
        }

        // 적 유닛인지 확인하고 데미지 적용
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage); // 적 유닛에 데미지 적용
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null && !enemyControllers.Contains(enemyController))
            {
                enemyControllers.Add(enemyController);
            }
        }
    }

    public void UnregisterEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null && enemyControllers.Contains(enemyController))
            {
                enemyControllers.Remove(enemyController);
            }
        }
    }
}
