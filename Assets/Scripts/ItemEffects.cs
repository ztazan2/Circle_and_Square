using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffects : MonoBehaviour
{
    [Header("프리팹 참조")]
    public GameObject lightningPrefab;         
    public GameObject typhoonPrefab;

    [Header("적 비활성화 지속 시간 ")]
    public float disableDuration;

    [Header("유닛 생성 쿨타임 무시 지속 시간 ")]
    public float bypassCooldownDuration;       

    [Header("스크립트 참조")]
    public UnitSummonManager unitSummonManager; 
    public ResourceManager resourceManager;
    public PlayerSkill playerSkill;

    [Header("자원량을 표시하는 텍스트")]
    public Text costText; 

    [Header("아이템 비용")]
    public int lightningCost;      
    public int typhoonCost;        
    public int disableEnemiesCost; 
    public int bypassCannonCost;   
    public int bypassSummonCost;   

    [Header("아이템 버튼")]
    public Button lightningButton;      
    public Button typhoonButton;        
    public Button disableEnemiesButton; 
    public Button bypassCannonButton;   
    public Button bypassSummonButton;   

    [Header("아이템 비용 텍스트 연결")]
    public Text lightningCostText;      
    public Text typhoonCostText;        
    public Text disableEnemiesCostText; 
    public Text bypassCannonCostText;   
    public Text bypassSummonCostText;   

    private void Start()
    {
        // 자원이 변경될 때마다 버튼 비용 텍스트를 갱신하도록 이벤트 구독
        resourceManager.OnResourceChanged += UpdateButtonCostTexts;
        UpdateCostUI();
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        resourceManager.OnResourceChanged -= UpdateButtonCostTexts;
    }

    // 효과 ID에 따라 해당 아이템 효과를 활성화하는 메서드
    public void ActivateItemEffect(int effectId)
    {
        CloseOpenPanel();

        if (!HasSufficientResource(effectId))
        {
            Debug.Log("자원이 부족하여 효과를 사용할 수 없습니다.");
            return;
        }

        DeductResource(effectId);
        UpdateCostUI();

        switch (effectId)
        {
            case 1:
                SummonLightning();
                break;
            case 2:
                SummonTyphoon();
                break;
            case 3:
                DisableAllEnemies(disableDuration);
                break;
            case 4:
                BypassCannonCooldown();
                break;
            case 5:
                StartCoroutine(unitSummonManager.BypassAllCooldowns(bypassCooldownDuration));
                Debug.Log($"{bypassCooldownDuration}초 동안 모든 소환 쿨타임 무시 효과가 적용되었습니다!");
                break;
        }
    }

    // 지정한 효과에 필요한 자원이 충분한지 검사하는 메서드
    bool HasSufficientResource(int effectId)
    {
        int requiredResource = GetEffectCost(effectId);
        return resourceManager.currentResource >= requiredResource;
    }

    // 효과 사용 시 자원을 차감하고 UI를 갱신하는 메서드
    void DeductResource(int effectId)
    {
        resourceManager.currentResource -= GetEffectCost(effectId);
        resourceManager.UpdateResourceUI();
    }

    // 효과 ID에 따른 비용을 반환하는 메서드
    int GetEffectCost(int effectId)
    {
        switch (effectId)
        {
            case 1: return lightningCost;
            case 2: return typhoonCost;
            case 3: return disableEnemiesCost;
            case 4: return bypassCannonCost;
            case 5: return bypassSummonCost;
            default: return 0;
        }
    }

    // 번개 효과를 활성화하는 메서드 (번개 소환)
    void SummonLightning()
    {
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f); // 소환 위치 (필요에 따라 수정 가능)
        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("번개 소환!");
    }

    // 태풍 효과를 활성화하는 메서드 (태풍 소환)
    void SummonTyphoon()
    {
        Vector3 startPosition = new Vector3(-6f, -1.4f, 0f);
        Vector3 targetPosition = new Vector3(6f, -1.4f, 0f);
        GameObject typhoon = Instantiate(typhoonPrefab, startPosition, Quaternion.identity);
        Debug.Log("태풍 소환!");

        TyphoonController typhoonController = typhoon.GetComponent<TyphoonController>();
        if (typhoonController != null)
        {
            typhoonController.Initialize(targetPosition);
        }
    }

    // 모든 적을 지정 시간 동안 비활성화하는 메서드
    void DisableAllEnemies(float duration)
    {
        Debug.Log("모든 적을 지정 시간 동안 비활성화합니다!");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.DisableEnemy(duration);
            }
        }
    }

    // 캐논 쿨타임 무시 효과를 활성화하는 메서드
    void BypassCannonCooldown()
    {
        Debug.Log("캐논 쿨타임 무시 효과 적용!");
        playerSkill.BypassCooldown();
    }

    // 현재 열려 있는 패널이 있다면 닫는 메서드
    void CloseOpenPanel()
    {
        if (PanelManager.Instance != null)
        {
            PanelManager.Instance.ClosePanel();
            Debug.Log("열려 있는 패널을 닫습니다.");
        }
    }

    // 전체 자원 UI 및 버튼 비용 텍스트를 갱신하는 메서드
    private void UpdateCostUI()
    {
        if (costText != null)
        {
            costText.text = "Resource: " + resourceManager.currentResource;
        }
        UpdateButtonCostTexts();
    }

    // 각 효과 버튼에 대한 비용 텍스트 및 활성화 상태를 갱신하는 메서드
    private void UpdateButtonCostTexts()
    {
        SetButtonState(lightningButton, lightningCostText, lightningCost);
        SetButtonState(typhoonButton, typhoonCostText, typhoonCost);
        SetButtonState(disableEnemiesButton, disableEnemiesCostText, disableEnemiesCost);

        // 캐논 쿨타임 무시 효과 버튼: 자원이 충분하고, 스킬 쿨타임이 완료되지 않았을 경우에만 활성화
        bool canUseCannonSkill = resourceManager.currentResource >= bypassCannonCost && !playerSkill.IsCooldownComplete();
        bypassCannonButton.interactable = canUseCannonSkill;
        bypassCannonCostText.text = $"{bypassCannonCost} GOLD";
        bypassCannonCostText.color = canUseCannonSkill ? Color.yellow : Color.gray;

        SetButtonState(bypassSummonButton, bypassSummonCostText, bypassSummonCost);
    }

    // 버튼의 활성화 여부와 비용 텍스트, 텍스트 색상을 설정하는 헬퍼 메서드
    private void SetButtonState(Button button, Text costText, int cost)
    {
        bool canAfford = resourceManager.currentResource >= cost;
        button.interactable = canAfford;
        costText.text = $"{cost} GOLD";
        costText.color = canAfford ? Color.yellow : Color.gray;
    }
}
