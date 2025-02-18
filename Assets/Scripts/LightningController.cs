using UnityEngine;

public class LightningController : MonoBehaviour
{
    public float damageRadius = 5f; // 번개 피해 반경
    public int damageAmount = 50; // 적에게 입힐 피해량
    public float duration = 0.5f; // 번개 효과 지속 시간
    public string enemyTag = "enemy"; // 적 태그

    private void Start()
    {
        // 번개 생성 시 바로 범위 내의 적에게 피해 적용
        ApplyDamageToEnemies();
        // duration 시간 후 번개 오브젝트 파괴
        Destroy(gameObject, duration);
    }

    private void ApplyDamageToEnemies()
    {
        // 씬 내의 지정된 태그를 가진 모든 적 오브젝트 검색
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);

            // 적이 피해 반경 내에 있다면
            if (distance <= damageRadius)
            {
                EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(damageAmount); // 피해 적용
                    Debug.Log($"{enemyObject.name}에게 {damageAmount}의 피해를 입혔습니다.");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 에디터 상에서 번개 피해 반경을 시각적으로 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
