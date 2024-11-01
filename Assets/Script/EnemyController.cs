using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float attackDamage = 50f; // ���� ���� ������
    public float attackRange = 1.5f;
    public float moveSpeed = 1.5f; // �̵� �ӵ�
    public float attackCooldown = 1f;
    public float maxHealth = 150f; // �ִ� ü��
    private float currentHealth;
    public List<GameObject> targets; // ���� ��� ����Ʈ (�÷��̾� �� ���� ����)

    public Text healthText; // ü���� ǥ���� Text ������Ʈ

    private float lastAttackTime = 0f;
    private Transform target;

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
        UpdateHealthText(); // �ʱ� ü�� ǥ��
    }

    void Update()
    {
        FindClosestTarget();

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // ���� ������ ������ �ʾ��� �� ��󿡰� �̵�
            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                // ���� ���� ���� �ְ�, ���� ��� �ð��� ������ �� ����
                lastAttackTime = Time.time;
                Attack(target.gameObject);
            }
        }
    }

    // ���� �������� �޾��� �� ȣ��Ǵ� �޼���
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("�� ü��: " + currentHealth); // ü�� ���� �޽��� ���
        UpdateHealthText(); // ü�� �ؽ�Ʈ ������Ʈ
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

    // ���� ������� �� ȣ��Ǵ� �޼���
    private void Die()
    {
        Debug.Log("�� ���");
        Destroy(gameObject);
    }

    // ����Ʈ���� ���� ����� �÷��̾� �Ǵ� ������ ã��
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

    // Ÿ�ٿ��� �̵�
    void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    // Ÿ���� ����
    void Attack(GameObject targetObject)
    {
        Debug.Log("���� ������ �����մϴ�: " + targetObject.name);

        // ����� �������� Ȯ���ϰ� ������ ����
        base_player basePlayer = targetObject.GetComponent<base_player>();
        if (basePlayer != null)
        {
            basePlayer.TakeDamage((int)attackDamage); // ������ ������
            return;
        }

        // ����� �÷��̾����� Ȯ���ϰ� ������ ����
        PlayerController playerController = targetObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage); // �÷��̾ ������
        }
    }

    // ���õ� ���¿����� ���� ������ �ð������� ǥ��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
