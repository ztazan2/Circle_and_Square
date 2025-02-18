using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int maxResource = 1000; // 최대 보유 가능 자원량
    public int currentResource = 0; // 현재 보유 자원량
    public int resourceRecoveryRate = 100; // 자원 회복량 (회복 주기마다 증가하는 자원량)
    public float recoveryInterval = 3f; // 자원 회복 주기 (초 단위)

    public Text resourceText; // 자원량을 표시할 UI 텍스트

    public event Action OnResourceChanged; // 자원 변경 이벤트 (UI 업데이트 등의 이벤트를 연결 가능)

    [System.Serializable]
    public class UnitResourceReward
    {
        public GameObject unitPrefab; // 적 유닛 프리팹
        public int resourceReward; // 해당 적을 처치 시 획득하는 자원량
    }

    public List<UnitResourceReward> unitResourceRewards = new List<UnitResourceReward>(5); // 유닛 처치 보상 목록

    private void Start()
    {
        // 일정 간격마다 자원을 회복하는 메서드 실행 (recoveryInterval 초마다 실행)
        InvokeRepeating(nameof(RecoverResource), recoveryInterval, recoveryInterval);
        UpdateResourceUI(); // UI 초기 업데이트
    }

    // 자원을 자동으로 회복하는 메서드
    private void RecoverResource()
    {
        currentResource = Mathf.Min(currentResource + resourceRecoveryRate, maxResource); // 최대치를 초과하지 않도록 제한
        UpdateResourceUI(); // UI 업데이트
    }

    // 자원 UI를 업데이트하는 메서드
    public void UpdateResourceUI()
    {
        resourceText.text = $"{currentResource} / {maxResource}"; // 현재 자원 상태 출력
        OnResourceChanged?.Invoke(); // 자원 변경 이벤트 호출 (UI 업데이트 등을 위한 이벤트)
    }

    // 적 유닛이 처치되었을 때 호출되는 메서드
    public void OnEnemyDefeated(GameObject enemy)
    {
        foreach (var unitReward in unitResourceRewards)
        {
            // (Clone) 제거 후 비교 (프리팹 이름과 비교하기 위해)
            string enemyName = enemy.name.Replace("(Clone)", "").Trim();

            // 적 유닛의 태그가 "enemy"이고, 프리팹 이름이 일치하면 보상 지급
            if (enemy.CompareTag("enemy") && enemyName == unitReward.unitPrefab.name)
            {
                AddResource(unitReward.resourceReward); // 자원 추가
                Debug.Log($"{enemy.name} 처치로 {unitReward.resourceReward} 자원 획득!");
                break;
            }
        }
    }

    // 특정 양의 자원을 추가하는 메서드
    private void AddResource(int amount)
    {
        currentResource = Mathf.Min(currentResource + amount, maxResource); // 최대 자원량 초과 방지
        UpdateResourceUI(); // UI 업데이트
    }
}
