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

        Debug.Log("�÷��̾� ü��: " + currentHealth);
        UpdateHealthText();
    }

    private void Die()
    {
        Debug.Log("�÷��̾� ���");
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
            Debug.LogWarning("healthText�� ������� �ʾҽ��ϴ�.");
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
        Debug.Log("�÷��̾ ������ �����մϴ�: " + enemy.name);

        // �������� Ȯ���ϰ� ������ ����
        base_enemy baseEnemy = enemy.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage); // attackDamage�� int�� ��ȯ�Ͽ� ������ ������ ����
            return;
        }

        // �� �������� Ȯ���ϰ� ������ ����
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage); // �� ���ֿ� ������ ����
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
