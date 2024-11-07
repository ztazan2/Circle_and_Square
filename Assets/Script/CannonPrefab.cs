using UnityEngine;

public class CannonPrefab : MonoBehaviour
{
    public float damageRadius;
    public int damageAmount;
    public float knockbackForce; // �˹� �� ����
    public string enemyTag;
    public float explodeDelay; // ���� ���� �ð�, ��ø� ����x

    private void Start()
    {
        Invoke(nameof(Explode), explodeDelay); // explodeDelay �� �� ����
    }

    public void Explode()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);
            if (distance <= damageRadius)
            {
                EnemyController enemy = enemyObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                    Debug.Log($"{enemyObject.name}�� {damageAmount}�� �������� ��"); // �������� �־��� �� �޽��� ���

                    // �˹� ���� ��� (�������� �� ��������)
                    Vector2 knockbackDirection = (enemyObject.transform.position - transform.position).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce);
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
