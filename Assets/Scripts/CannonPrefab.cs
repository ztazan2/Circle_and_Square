using UnityEngine;

public class CannonPrefab : MonoBehaviour
{
    public float damageRadius; // 캐논 폭발 피해 반경
    public int damageAmount; // 캐논 폭발 시 입힐 피해량
    public float knockbackForce; // 넉백(밀림) 힘
    public string enemyTag; // 적 태그
    public float explodeDelay; // 폭발 지연 시간

    private void Start()
    {
        // 지정된 지연 시간 후 Explode() 메서드 호출
        Invoke(nameof(Explode), explodeDelay);
    }

    public void Explode()
    {
        // 지정된 태그를 가진 적 오브젝트들을 모두 검색
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // 검색된 적들에게 폭발 피해 및 넉백 효과 적용
        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);
            if (distance <= damageRadius) // 폭발 반경 내에 있는 경우
            {
                EnemyController enemy = enemyObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount); // 피해 적용
                    Debug.Log($"{enemyObject.name}에게 {damageAmount}의 피해를 입혔습니다."); // 피해 로그 출력

                    // 넉백 효과 적용: 현재 x축 오른쪽 방향으로만 적용
                    Vector2 knockbackDirection = new Vector2(1, 0).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce);
                    Debug.Log($"{enemyObject.name}에게 x축 방향으로 {knockbackForce}의 넉백 효과를 적용했습니다.");
                }
            }
        }

        Destroy(gameObject); // 캐논 프리팹 오브젝트 파괴
    }

    private void OnDrawGizmosSelected()
    {
        // 에디터에서 폭발 피해 반경을 시각적으로 확인할 수 있도록 Gizmo 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
