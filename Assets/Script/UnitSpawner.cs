using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSummonManager : MonoBehaviour
{
    public ResourceManager resourceManager; // ResourceManager 참조
    public Text resourceText; // 현재 자원 상태 텍스트

    [System.Serializable]
    public class UnitButton
    {
        public Button summonButton; // 유닛 소환 버튼
        public Text costText; // 버튼에 표시될 자원 비용 텍스트
        public int summonCost; // 유닛 소환 비용
        public GameObject unitPrefab; // 소환할 유닛 프리팹
        public float cooldownTime; // 유닛 소환 쿨타임 (초 단위)
        [HideInInspector] public float lastSummonTime; // 마지막 소환 시간
    }

    public List<UnitButton> unitButtons = new List<UnitButton>(5); // 5개의 유닛 소환 버튼 설정
    private Vector3 spawnPosition = new Vector3(-6f, -1.4f, 0f); // 유닛 소환 위치 고정

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 현재 자원 상태 표시
        resourceText.text = $"{resourceManager.currentResource} / {resourceManager.maxResource}";

        // 각 버튼의 상태 업데이트
        foreach (UnitButton unitButton in unitButtons)
        {
            bool canSummon = resourceManager.currentResource >= unitButton.summonCost &&
                             Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime;
            unitButton.summonButton.image.color = canSummon ? Color.yellow : Color.gray;
            unitButton.costText.text = $"{unitButton.summonCost} GOLD";
        }
    }

    public void SummonUnit(int index)
    {
        if (index >= 0 && index < unitButtons.Count)
        {
            UnitButton unitButton = unitButtons[index];

            // 소환 가능 여부 확인 (자원 조건과 쿨타임 조건)
            if (resourceManager.currentResource >= unitButton.summonCost &&
                Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime)
            {
                resourceManager.currentResource -= unitButton.summonCost;
                unitButton.lastSummonTime = Time.time; // 현재 시간을 마지막 소환 시간으로 설정
                Debug.Log($"{unitButton.summonButton.name} 유닛 소환!");
                UpdateUI();

                // 유닛 소환 로직
                if (unitButton.unitPrefab != null)
                {
                    Instantiate(unitButton.unitPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
