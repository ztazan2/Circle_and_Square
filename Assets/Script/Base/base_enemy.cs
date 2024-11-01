using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class base_enemy : MonoBehaviour
{
    public int maxHealth = 9000; // ������ ��ü��
    private int currentHealth; // ���� ü��
    public Text healthText; // ü���� ǥ���� �ؽ�Ʈ UI

    void Start()
    {
        // ü���� �ʱ�ȭ�ϰ�, �ؽ�Ʈ UI�� ������Ʈ�մϴ�.
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    // �÷��̾��� �������� ���� ������ ���ظ� ���� �� ȣ���� �޼���
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // ü���� 0 �̸����� �������� �ʵ��� ����
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }

        // ü�� �ؽ�Ʈ UI ������Ʈ
        UpdateHealthText();

        // ���� ü���� 0�� �Ǹ� ������ �ı���
        if (currentHealth == 0)
        {
            DestroyBase();
        }
    }

    // ü�� �ؽ�Ʈ UI�� ������Ʈ�ϴ� �޼���
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    // ������ �ı��Ǿ��� �� ������ ���� (��: �ı� �ִϸ��̼�, ���� ���� ó�� ��)
    private void DestroyBase()
    {
        // ���� �ı� ���� �ۼ� (�ʿ��� ���)
        Debug.Log("������ �ı��Ǿ����ϴ�!");
        // �̰��� �ı� �ִϸ��̼��̳� ���� ���� ó�� �ڵ带 �߰��� �� �ֽ��ϴ�.
    }
}
