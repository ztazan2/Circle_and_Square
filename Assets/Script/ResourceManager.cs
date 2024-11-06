using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int maxResource = 1000;  // �ִ� �ڿ���
    public int currentResource = 0; // ���� �ڿ���
    public int resourceRecoveryRate = 100; // �ڿ� ȸ����
    public float recoveryInterval = 3f; // ȸ�� ����

    public Text resourceText; // �ڿ� ���� ǥ�ÿ� �ؽ�Ʈ

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
            if (enemy.CompareTag("enemy") && enemy.name.StartsWith(unitReward.unitPrefab.name)) // �̸��� ���λ�� ��
            {
                AddResource(unitReward.resourceReward);
                Debug.Log($"{enemy.name} óġ�� {unitReward.resourceReward} �ڿ� ȹ��!");
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
