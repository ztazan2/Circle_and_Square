using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float health = 100f; // �ʱ� ü��

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log("�� ü��: " + health); // ü�� ���� �޽��� ���

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("�� ���"); // �� ��� �޽��� ���
        Destroy(gameObject);
    }
}
