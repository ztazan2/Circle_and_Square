using UnityEngine;

public class CannonPrefab : MonoBehaviour
{
    public float damageRadius;
    public int damageAmount;
    public float knockbackForce; // 넉백 힘 설정
    public string enemyTag;
    public float explodeDelay; // 폭발 지연 시간, 즉시면 등장x

    private void Start()
    {
        Invoke(nameof(Explode), explodeDelay); // explodeDelay 초 후 폭발
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
                    Debug.Log($"{enemyObject.name}에 {damageAmount}의 데미지를 줌"); // 데미지를 주었을 때 메시지 출력

                    // 넉백 방향 계산 (대포에서 적 방향으로)
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
