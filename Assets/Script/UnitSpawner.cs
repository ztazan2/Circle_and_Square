using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSummonManager : MonoBehaviour
{
    public ResourceManager resourceManager; // ResourceManager ����
    public Text resourceText; // ���� �ڿ� ���� �ؽ�Ʈ

    [System.Serializable]
    public class UnitButton
    {
        public Button summonButton; // ���� ��ȯ ��ư
        public Text costText; // ��ư�� ǥ�õ� �ڿ� ��� �ؽ�Ʈ
        public int summonCost; // ���� ��ȯ ���
        public GameObject unitPrefab; // ��ȯ�� ���� ������
        public float cooldownTime; // ���� ��ȯ ��Ÿ�� (�� ����)
        public Image cooldownOverlay; // ��Ÿ�� ǥ�ÿ� �������� �̹���
        [HideInInspector] public float lastSummonTime; // ������ ��ȯ �ð�
        [HideInInspector] public bool isFirstSummon = true; // ù ��ȯ ����
    }

    public List<UnitButton> unitButtons = new List<UnitButton>(5); // 5���� ���� ��ȯ ��ư ����
    private Vector3 spawnPosition = new Vector3(-6f, -1.4f, 0f); // ���� ��ȯ ��ġ ����

    private void Start()
    {
        foreach (UnitButton unitButton in unitButtons)
        {
            unitButton.cooldownOverlay.fillAmount = 1; // �ʱ� ���¿��� ��Ÿ�� �������̰� ���� �� ����
        }
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // ���� �ڿ� ���� ǥ��
        resourceText.text = $"{resourceManager.currentResource} / {resourceManager.maxResource}";

        // �� ��ư�� ���� ������Ʈ
        foreach (UnitButton unitButton in unitButtons)
        {
            bool canSummon = resourceManager.currentResource >= unitButton.summonCost &&
                             (unitButton.isFirstSummon || Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime);

            // ��ư ���� ����
            unitButton.summonButton.image.color = canSummon ? Color.yellow : Color.gray;
            unitButton.costText.text = $"{unitButton.summonCost} GOLD";

            // ��Ÿ�� ���������� fillAmount ����
            if (!canSummon && !unitButton.isFirstSummon)
            {
                // ù ��ȯ ���� ��Ÿ���� ����ǵ��� ����
                float cooldownProgress = (Time.time - unitButton.lastSummonTime) / unitButton.cooldownTime;
                unitButton.cooldownOverlay.fillAmount = Mathf.Clamp01(cooldownProgress);
            }
            else
            {
                unitButton.cooldownOverlay.fillAmount = 1; // ��Ÿ���� ������ �������̰� ���� �� ���·� ����
            }
        }
    }

    public void SummonUnit(int index)
    {
        if (index >= 0 && index < unitButtons.Count)
        {
            UnitButton unitButton = unitButtons[index];

            // ��ȯ ���� ���� Ȯ�� (�ڿ� ���ǰ� ��Ÿ�� ����)
            bool canSummon = resourceManager.currentResource >= unitButton.summonCost &&
                             (unitButton.isFirstSummon || Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime);

            if (canSummon)
            {
                resourceManager.currentResource -= unitButton.summonCost;
                unitButton.lastSummonTime = Time.time; // ���� �ð��� ������ ��ȯ �ð����� ����
                unitButton.isFirstSummon = false; // ù ��ȯ ���� ��Ÿ�� ����
                unitButton.cooldownOverlay.fillAmount = 0; // Ŭ�� �� ��Ÿ�� �ʱ�ȭ
                Debug.Log($"{unitButton.summonButton.name} ���� ��ȯ!");
                UpdateUI();

                // ���� ��ȯ ����
                if (unitButton.unitPrefab != null)
                {
                    Instantiate(unitButton.unitPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
