using UnityEngine;
using System.Collections.Generic;

public class TyphoonController : MonoBehaviour
{
    public float speed; // 태풍의 이동 속도
    public float knockbackForce; // 적들에게 적용할 넉백(밀려나는) 힘
    public float knockbackRadius; // 넉백이 적용되는 반경
    public string enemyTag = "enemy"; // 적 태그 (적을 구별하기 위한 태그)
    private Vector3 targetPosition; // 목표 위치 (태풍이 이동할 최종 목적지)

    private HashSet<GameObject> knockedBackEnemies = new HashSet<GameObject>(); // 이미 넉백된 적들을 추적하는 Set

    // 태풍을 초기화하는 메서드 (목표 위치 설정)
    public void Initialize(Vector3 targetPos)
    {
        targetPosition = targetPos;

        // 태풍의 Z 좌표를 고정하여 플레이어 시점에서 잘 보이도록 설정
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
    }

    void Update()
    {
        // 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // 목표 위치에 도달하면 태풍 오브젝트 파괴
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }

        // 넉백 효과 적용
        ApplyKnockbackInRadius();
    }

    // 넉백 효과를 반경 내의 적들에게 적용하는 메서드
    private void ApplyKnockbackInRadius()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); // 현재 씬에서 적 오브젝트 검색
        foreach (GameObject enemyObject in enemies)
        {
            // 이미 넉백된 적이면 처리하지 않음
            if (knockedBackEnemies.Contains(enemyObject)) continue;

            // 태풍과 적과의 거리 계산
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);
            if (distance <= knockbackRadius)
            {
                // 적의 EnemyController 컴포넌트를 가져옴
                EnemyController enemy = enemyObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    // 넉백 방향 설정 (현재 오른쪽 방향으로만 적용됨)
                    Vector2 knockbackDirection = new Vector2(1, 0).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce); // 넉백 적용
                    knockedBackEnemies.Add(enemyObject); // 넉백된 적을 목록에 추가
                    Debug.Log($"{enemyObject.name}에 태풍의 넉백 효과 적용");
                }
            }
        }
    }

    // 태풍의 넉백 반경을 에디터에서 시각적으로 표시하는 기즈모 함수
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, knockbackRadius);
    }
}
