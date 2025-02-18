using UnityEngine;
using UnityEngine.UI;

public class base_enemy : MonoBehaviour
{
    // 현재 체력
    private int currentHealth;
    [Header("최대 체력")] public int maxHealth;
    [Header("체력 UI를 표시하는 텍스트")] public Text healthText;

    private EedPanel endPanel;

    void Start()
    {
        // 체력을 초기화하고 체력 UI를 갱신
        currentHealth = maxHealth;
        UpdateHealthText();

        // 씬 내에서 EedPanel 찾기
        endPanel = FindObjectOfType<EedPanel>();
    }

    // 적에게 피해를 입혔을 때 호출
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // 체력이 0 이하가 되면 기지가 파괴
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyBase();
        }

        Debug.Log("기지 체력: " + currentHealth);

        // 체력 UI를 갱신합니다.
        UpdateHealthText();
    }

    // 체력 UI를 갱신
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    // 기지가 파괴되었을 때 호출
    private void DestroyBase()
    {
        Debug.Log("에너미 기지가 파괴되었습니다!");

        // 게임 종료를 처리
        if (endPanel != null)
        {
            endPanel.EndGame(false); 
        }

        Destroy(gameObject); 
    }
}
