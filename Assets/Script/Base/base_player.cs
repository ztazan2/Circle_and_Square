using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class base_player : MonoBehaviour
{
    public int maxHealth; // 기본 최대 체력
    private int currentHealth; // 현재 체력
    public Text healthText; // 체력을 표시할 텍스트 UI

    private EndGamePanelController gameManager; // GameManager 참조

    void Start()
    {
        // 초기 체력을 설정하고 텍스트 UI 업데이트
        currentHealth = maxHealth;
        UpdateHealthText();

        // GameManager 찾기
        gameManager = FindObjectOfType<EndGamePanelController>();
    }

    // 데미지를 받아 체력을 감소시키는 메서드
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // 체력이 0 미만으로 내려가지 않도록 설정
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyBase();
        }

        Debug.Log("플레이어 기지 체력 감소: " + currentHealth);

        // 체력 텍스트 UI 업데이트
        UpdateHealthText();
    }

    // 체력 텍스트 UI를 업데이트하는 메서드
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    // 기지가 파괴되었을 때 실행할 동작
    private void DestroyBase()
    {
        Debug.Log("플레이어 기지가 파괴되었습니다!");

        // GameManager를 통해 게임 종료 처리
        if (gameManager != null)
        {
            gameManager.EndGame(true); // 플레이어 기지 파괴 시 true 전달
        }

        Destroy(gameObject);
    }
}
