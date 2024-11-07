using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public float attackDamage; // ���� ������
    public float attackRange; // ���� ��Ÿ�
    public float moveSpeed; // �̵� �ӵ�
    public float attackCooldown; // ���� ��� �ð�
    public float maxHealth; // �ִ� ü��
    private float currentHealth; // ���� ü��
    public float knockbackForce; // �˹� ��
    public Text healthText; // ü���� ǥ���� Text ������Ʈ

    [Header("Targeting Settings")]
    public List<string> targetTags; // ������ �±� ����Ʈ
    public List<string> attackPriorityTags; // ���� �켱���� �±� ����Ʈ

    private Transform target; // ���� ����� Transform
    private float lastAttackTime; // ������ ���� �ð��� ����
    private bool knockbackApplied = false; // �˹� �� ���� ����ǵ��� ����

    private ResourceManager resourceManager; // ResourceManager ����

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
        resourceManager = FindObjectOfType<ResourceManager>(); // ResourceManager �˻�
        UpdateHealthText(); // �ʱ� ü�� ǥ��
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
            // �⺻ �˹��� �����ϱ� ���� ApplyKnockback ȣ��
            ApplyKnockback(Vector2.right, knockbackForce); // �׻� ���������θ� �и����� ����
            knockbackApplied = true; // �˹��� �� ���� ����ǵ��� ����
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("�� ü��: " + currentHealth);
        UpdateHealthText(); // ü�� �ؽ�Ʈ ������Ʈ
    }

    // �ܺο��� �˹� ����� ���� �޾� ��ġ�� �����ϴ� �޼���
    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        // �˹� ������ ������(x�� ��� ����)���� ���� ����
        knockbackDirection = Vector2.right;

        transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0"); // ü�� �ؽ�Ʈ ������Ʈ
        }
        else
        {
            Debug.LogWarning("healthText�� ������� �ʾҽ��ϴ�.");
        }
    }

    private void Die()
    {
        Debug.Log("�� ���");

        // ���� ����� �� ResourceManager�� ���� ȣ��
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

        // �켱������ ���� ��� Ž��
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
            // �켱���� �±׿� ����� ������ ã�� ����
            if (target != null)
            {
                return;
            }
        }

        // �켱���� �±׿��� ����� ã�� ������ ��� �Ϲ� ��� �±׿��� ã��
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
        Debug.Log("���� ������ �����մϴ�: " + targetObject.name);

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
