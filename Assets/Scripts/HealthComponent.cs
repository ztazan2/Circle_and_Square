using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f; // 최대 체력
    protected float currentHealth; // 현재 체력

    // 외부에서 currentHealth 값을 읽기 위한 프로퍼티
    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth; // 시작 시 현재 체력을 최대 체력으로 초기화
    }

    // 피해를 입는 메서드
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " 체력: " + currentHealth); // 체력 변화 로그 출력

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 사망 처리 메서드 (가상 메서드로 상속받아 재정의 가능)
    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " 사망"); // 사망 시 로그 출력
        Destroy(gameObject);
    }
}
