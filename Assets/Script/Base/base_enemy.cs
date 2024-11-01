using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class base_enemy : MonoBehaviour
{
    public int maxHealth = 9000; // 기지의 총체력
    private int currentHealth; // 현재 체력
    public Text healthText; // 체력을 표시할 텍스트 UI

    void Start()
    {
        // 체력을 초기화하고, 텍스트 UI를 업데이트합니다.
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    // 플레이어의 공격으로 인해 기지가 피해를 입을 때 호출할 메서드
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // 체력이 0 미만으로 내려가지 않도록 설정
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }

        // 체력 텍스트 UI 업데이트
        UpdateHealthText();

        // 기지 체력이 0이 되면 기지가 파괴됨
        if (currentHealth == 0)
        {
            DestroyBase();
        }
    }

    // 체력 텍스트 UI를 업데이트하는 메서드
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    // 기지가 파괴되었을 때 실행할 동작 (예: 파괴 애니메이션, 게임 오버 처리 등)
    private void DestroyBase()
    {
        // 기지 파괴 로직 작성 (필요한 경우)
        Debug.Log("기지가 파괴되었습니다!");
        // 이곳에 파괴 애니메이션이나 게임 종료 처리 코드를 추가할 수 있습니다.
    }
}
