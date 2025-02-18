using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSummonManager : MonoBehaviour
{
    public ResourceManager resourceManager; // ResourceManager 참조
    public Text resourceText; // 현재 자원량을 표시할 UI 텍스트

    [System.Serializable]
    public class UnitButton
    {
        public Button summonButton; // 유닛 소환 버튼
        public Text costText; // 버튼에 표시될 유닛 소환 비용 텍스트
        public int summonCost; // 유닛 소환에 필요한 비용
        public GameObject unitPrefab; // 소환할 유닛 프리팹
        public float cooldownTime; // 유닛 소환 쿨타임 (초 단위)
        public Image cooldownOverlay; // 쿨타임 진행 상태를 표시할 오버레이 이미지
        [HideInInspector] public float lastSummonTime; // 마지막으로 유닛을 소환한 시간
        [HideInInspector] public bool isFirstSummon = true; // 첫 번째 소환 여부 (처음 소환 시 쿨타임 무시)
    }

    public List<UnitButton> unitButtons = new List<UnitButton>(5); // 최대 5개의 유닛 소환 버튼 설정
    private Vector3 spawnPosition = new Vector3(-6f, -1.4f, 0f); // 유닛이 소환될 위치
    private bool isCooldownBypassed = false; // 쿨타임 무시 여부 (특정 효과에 의해 일시적으로 쿨타임 무시 가능)

    private void Start()
    {
        // 초기화: 모든 소환 버튼의 쿨타임 오버레이를 완전히 채운 상태로 설정
        foreach (UnitButton unitButton in unitButtons)
        {
            unitButton.cooldownOverlay.fillAmount = 1;
        }
        UpdateUI(); // UI 초기 업데이트
    }

    private void Update()
    {
        UpdateUI(); // 지속적으로 UI 갱신 (자원 및 버튼 상태 변경 반영)
    }

    // UI를 업데이트하는 메서드
    private void UpdateUI()
    {
        // 현재 자원량 표시
        resourceText.text = $"{resourceManager.currentResource} / {resourceManager.maxResource}";

        // 패널이 열려 있는지 확인하여 버튼을 비활성화할지 결정
        bool isPanelOpen = PanelManager.Instance != null && PanelManager.Instance.IsPanelOpen();

        // 모든 유닛 버튼에 대해 UI 갱신
        foreach (UnitButton unitButton in unitButtons)
        {
            // 소환 가능 여부 확인
            bool canSummon = resourceManager.currentResource >= unitButton.summonCost &&
                             (unitButton.isFirstSummon || isCooldownBypassed || Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime);

            // 패널이 열려 있으면 버튼 비활성화
            unitButton.summonButton.interactable = !isPanelOpen && canSummon;
            unitButton.summonButton.image.color = unitButton.summonButton.interactable ? Color.yellow : Color.gray;
            unitButton.costText.text = $"{unitButton.summonCost} GOLD"; // 비용 텍스트 갱신

            // 쿨타임 진행 상태를 표시
            if (!canSummon && !unitButton.isFirstSummon && !isCooldownBypassed)
            {
                float cooldownProgress = (Time.time - unitButton.lastSummonTime) / unitButton.cooldownTime;
                unitButton.cooldownOverlay.fillAmount = Mathf.Clamp01(cooldownProgress);
            }
            else
            {
                unitButton.cooldownOverlay.fillAmount = 1; // 쿨타임이 끝났다면 완전한 상태로 표시
            }
        }
    }

    // 유닛을 소환하는 메서드
    public void SummonUnit(int index)
    {
        // 인덱스가 유효한지 확인
        if (index >= 0 && index < unitButtons.Count)
        {
            UnitButton unitButton = unitButtons[index];

            // 소환 가능 여부 확인
            bool canSummon = resourceManager.currentResource >= unitButton.summonCost &&
                             (unitButton.isFirstSummon || isCooldownBypassed || Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime);

            if (canSummon)
            {
                // 자원 차감
                resourceManager.currentResource -= unitButton.summonCost;
                unitButton.lastSummonTime = Time.time; // 소환 시간 기록
                unitButton.isFirstSummon = false; // 첫 번째 소환 이후부터는 쿨타임 적용
                unitButton.cooldownOverlay.fillAmount = 0; // 쿨타임 UI 초기화

                Debug.Log($"{unitButton.summonButton.name} 유닛 소환!");

                UpdateUI(); // UI 갱신

                // 유닛을 지정된 위치에 소환
                if (unitButton.unitPrefab != null)
                {
                    Instantiate(unitButton.unitPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    // 일정 시간 동안 모든 유닛의 쿨타임을 무시하는 효과를 부여하는 코루틴
    public IEnumerator BypassAllCooldowns(float duration)
    {
        isCooldownBypassed = true; // 쿨타임 무시 활성화
        yield return new WaitForSeconds(duration); // 지정된 시간(duration) 동안 유지
        isCooldownBypassed = false; // 쿨타임 무시 효과 해제
        Debug.Log("모든 유닛 소환 쿨타임 무시 효과 종료.");
    }
}
