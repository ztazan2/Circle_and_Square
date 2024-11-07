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
            // "(Clone)" ���̻縦 �����ϰ� �̸��� ���Ͽ� ��Ȯ�ϰ� ��Ī
            string enemyName = enemy.name.Replace("(Clone)", "").Trim();

            if (enemy.CompareTag("enemy") && enemyName == unitReward.unitPrefab.name)
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
