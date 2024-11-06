using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int maxResource = 1000;  // 최대 자원량
    public int currentResource = 0; // 현재 자원량
    public int resourceRecoveryRate = 100; // 자원 회복량
    public float recoveryInterval = 3f; // 회복 간격

    public Text resourceText; // 자원 상태 표시용 텍스트

    [System.Serializable]
    public class UnitResourceReward
    {
        public GameObject unitPrefab;
        public int resourceReward;
    }

    public List<UnitResourceReward> unitResourceRewards = new List<UnitResourceReward>(5);

    private void Start()
    {
        InvokeRepeating(nameof(RecoverResource), recoveryInterval, recoveryInterval);
        UpdateResourceUI();
    }

    private void RecoverResource()
    {
        currentResource = Mathf.Min(currentResource + resourceRecoveryRate, maxResource);
        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        resourceText.text = $"{currentResource} / {maxResource}";
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        foreach (var unitReward in unitResourceRewards)
        {
            if (enemy.CompareTag("enemy") && enemy.name.StartsWith(unitReward.unitPrefab.name)) // 이름이 접두사로 비교
            {
                AddResource(unitReward.resourceReward);
                Debug.Log($"{enemy.name} 처치로 {unitReward.resourceReward} 자원 획득!");
                break;
            }
        }
    }

    private void AddResource(int amount)
    {
        currentResource = Mathf.Min(currentResource + amount, maxResource);
        UpdateResourceUI();
    }
}
