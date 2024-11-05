using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float attackDamage; // ���� ������
    public float attackRange; // ���� ��Ÿ�
    public float moveSpeed; // �̵� �ӵ�
    public float attackCooldown; // ���� ��� �ð�
    public float maxHealth; // �ִ� ü��
    private float currentHealth; // ���� ü��
    public float knockbackForce; // �˹� ��
    public Text healthText; // ü���� ǥ���� Text ������Ʈ

    public List<GameObject> targets; // ���� ��� ����Ʈ (�� �� ���� ����)
    private Transform target; // ���� ���� ����� Transform

    private float lastAttackTime; // ������ ���� �ð��� ����
    private bool knockbackApplied = false; // �˹� �� ���� ����ǵ��� ����

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
            knockbackApplied = true; // �˹��� �� ���� ����ǵ��� ����
        }

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
        Debug.Log("�÷��̾ ������ �����մϴ�: " + targetObject.name);

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
            Debug.Log("�˹� �����");
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
            targets.Add(enemy); // ���ο� ���� ���� ��� ����Ʈ�� �߰�
            Debug.Log("���ο� ���� ��ϵǾ����ϴ�: " + enemy.name);
        }
    }
}
