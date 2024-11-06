using UnityEngine;
using UnityEngine.UI;

public class WorkerLevelManager : MonoBehaviour
{
    public int currentLevel = 1; // 현재 레벨
    public int maxLevel = 8; // 최대 레벨

    // ResourceManager를 참조
    private ResourceManager resourceManager;

    // UI 요소
    public Text levelText; // 다음 레벨 및 문구 표시용 텍스트
    public Text resourceText; // 현재 자원 상태 및 레벨업 필요 자원량 표시용 텍스트
    public Button levelUpButton; // 레벨업 버튼
    private Color defaultButtonColor = Color.gray; // 기본 버튼 색상을 회색으로 설정

    private readonly int[] levelUpRequirements = { 180, 360, 540, 720, 900, 1080, 1260 };

    private void Start()
    {
        // ResourceManager 컴포넌트를 찾고 참조 저장
        resourceManager = FindObjectOfType<ResourceManager>();

        // 버튼의 초기 색상 설정
        levelUpButton.image.color = defaultButtonColor;

        UpdateUI(); // 초기 UI 업데이트
    }

    private void Update()
    {
        UpdateUI(); // UI 업데이트
    }

    public void LevelUp()
    {
        if (CanLevelUp())
        {
            // 자원 차감
            resourceManager.currentResource -= levelUpRequirements[currentLevel - 1];
            currentLevel++;

            // 레벨업으로 인한 자원 회복 속도와 최대 자원량 증가
            resourceManager.resourceRecoveryRate += 20;
            resourceManager.maxResource += 500;

            // UI 업데이트 호출
            UpdateUI();
            Debug.Log($"레벨 {currentLevel}로 상승! 회복 속도: {resourceManager.resourceRecoveryRate}, 최대 자원량: {resourceManager.maxResource}");
        }
    }

    private bool CanLevelUp()
    {
        return currentLevel < maxLevel && resourceManager.currentResource >= levelUpRequirements[currentLevel - 1];
    }

    private void UpdateUI()
    {
        // 다음 레벨과 레벨업 문구 표시
        if (currentLevel < maxLevel)
        {
            levelText.text = $"Lv. {currentLevel + 1}\nLEVEL\nUP!\n{levelUpRequirements[currentLevel - 1]} GOLD";
        }
        else
        {
            levelText.text = "MAX LEVEL";
        }

        // 현재 자원 상태 표시
        resourceText.text = $"{resourceManager.currentResource} / {resourceManager.maxResource}";

        // 레벨업 가능 여부에 따라 버튼 색상 변경
        if (CanLevelUp())
        {
            levelUpButton.image.color = Color.yellow;
        }
        else
        {
            levelUpButton.image.color = defaultButtonColor; // 기본 색상 회색으로 복구
        }
    }
}
