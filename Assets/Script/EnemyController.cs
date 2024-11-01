using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float health = 100f; // 초기 체력

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("적 체력: " + health); // 체력 감소 메시지 출력

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("적 사망"); // 적 사망 메시지 출력
        Destroy(gameObject);
    }
}
