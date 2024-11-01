using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackDamage = 100f;
    public float attackRange = 1.5f;
    public float moveSpeed = 2f; // �̵� �ӵ�
    public float attackCooldown = 1f;
    public float maxHealth = 100f; // �ִ� ü��
    private float currentHealth;
    public List<GameObject> enemies; // �� ������Ʈ ����Ʈ (���� ����)

    public Text healthText; // ü���� ǥ���� Text ������Ʈ

    private float lastAttackTime = 0f;
    private Transform targetEnemy;

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
        UpdateHealthText(); // �ʱ� ü�� ǥ��
    }

    void Update()
    {
        FindClosestEnemy();

        if (targetEnemy != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, targetEnemy.position);

            // ���� ������ ������ �ʾ��� �� ������ �̵�
            if (distanceToEnemy > attackRange)
            {
                MoveTowardsEnemy();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                // ���� ���� ���� �ְ�, ���� ��� �ð��� ������ �� ����
                lastAttackTime = Time.time;
                Attack(targetEnemy.gameObject);
            }
        }
    }

    // �÷��̾ �������� �޾��� �� ȣ��Ǵ� �޼���
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("�÷��̾� ü��: " + currentHealth); // ü�� ���� �޽��� ���
        UpdateHealthText(); // ü�� �ؽ�Ʈ ������Ʈ
    }

    // �÷��̾ ������� �� ȣ��Ǵ� �޼���
    private void Die()
    {
        Debug.Log("�÷��̾� ���");
        Destroy(gameObject);
    }

    // ü�� �ؽ�Ʈ�� ������Ʈ�ϴ� �޼���
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0"); // �Ҽ��� ���� ǥ��
        }
        else
        {
            Debug.LogWarning("healthText�� ������� �ʾҽ��ϴ�.");
        }
    }

    // ����Ʈ���� ���� ����� �� �Ǵ� ������ ã��
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

    // ������ �̵�
    void MoveTowardsEnemy()
    {
        Vector2 direction = (targetEnemy.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetEnemy.position, moveSpeed * Time.deltaTime);
    }

    // �� �Ǵ� ������ ����
    void Attack(GameObject enemy)
    {
        Debug.Log("�÷��̾ ������ �����մϴ�: " + enemy.name);

        // �������� Ȯ���ϰ� ������ ����
        base_enemy baseEnemy = enemy.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage); // ������ ������
            return;
        }

        // ������ Ȯ���ϰ� ������ ����
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage); // ���� ������
        }
    }

    // ���õ� ���¿����� ���� ������ �ð������� ǥ��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
