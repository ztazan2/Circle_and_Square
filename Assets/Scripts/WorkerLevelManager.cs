using UnityEngine;
using UnityEngine.UI;

public class WorkerLevelManager : MonoBehaviour
{
    [Header("현재 레벨")] public int currentLevel = 1; 
    [Header("최대 레벨")] public int maxLevel = 8; 

    // UI 요소
    [Header("버튼에 표시될 텍스트")] public Text levelText;
    [Header("자원을 표시할 텍스트")] public Text resourceText; 
    [Header("레벨 업 버튼")] public Button levelUpButton;

    // 기본 버튼 색상을 회색으로 설정
    private Color defaultButtonColor = Color.gray;

    // 각 레벨 업에 필요한 자원의 양 (레벨 2~8)
    private readonly int[] levelUpRequirements = { 180, 360, 540, 720, 900, 1080, 1260 };

    private ResourceManager resourceManager;
    private void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();

        // 버튼 초기 색상 설정
        levelUpButton.image.color = defaultButtonColor;

        // 초기 UI 업데이트
        UpdateUI(); 
    }

    private void Update()
    {
        // 자원 변화에 따르 UI 업데이트 
        UpdateUI(); 
    }

    // 레벨 업 버튼 클릭 시 호출
    public void LevelUp()
    {
        if (CanLevelUp())
        {
            // 필요한 자원 차감
            resourceManager.currentResource -= levelUpRequirements[currentLevel - 1];
            // 레벨 증가
            currentLevel++; 

            // 레벨 업 시 자원 회복량 증가 및 최대 자원량 증가
            resourceManager.resourceRecoveryRate += 20;
            resourceManager.maxResource += 500;

            // UI 업데이트
            UpdateUI();
            Debug.Log($"레벨 {currentLevel}로 상승! 회복 속도: {resourceManager.resourceRecoveryRate}, 최대 자원량: {resourceManager.maxResource}");
        }
    }

    // 레벨 업 가능 여부를 확인
    private bool CanLevelUp()
    {
        return currentLevel < maxLevel && resourceManager.currentResource >= levelUpRequirements[currentLevel - 1];
    }

    // 상태 메시지 업데이트
    private void UpdateUI()
    {
        // 다음 레벨 및 요구 골드 표시
        if (currentLevel < maxLevel)
        {
            levelText.text = $"Lv. {currentLevel + 1}\nLEVEL\nUP!\n{levelUpRequirements[currentLevel - 1]} GOLD";
        }
        // 최대 레벨 도달 시 변경
        else
        {
            levelText.text = "MAX LEVEL"; 
        }

        // 현재 자원 표시
        resourceText.text = $"{resourceManager.currentResource} / {resourceManager.maxResource}";

        // 패널이 열려 있다면 버튼 비활성화
        if (PanelManager.Instance != null && PanelManager.Instance.IsPanelOpen())
        {
            levelUpButton.interactable = false;
            levelUpButton.image.color = defaultButtonColor;
        }
        else
        {
            // 레벨 업 가능 여부에 따라 버튼 상태 변경
            if (CanLevelUp())
            {
                levelUpButton.interactable = true;
                // 가능할 경우 버튼 색상 변경
                levelUpButton.image.color = Color.yellow; 
            }
            else
            {
                levelUpButton.interactable = false;
                // 불가하면 기본 색상 유지
                levelUpButton.image.color = defaultButtonColor; 
            }
        }
    }
}
