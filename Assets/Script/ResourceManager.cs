using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int maxResource = 1000;  // �ִ� �ڿ���
    public int currentResource = 0; // ���� �ڿ���
    public int resourceRecoveryRate = 100; // �ڿ� ȸ����
    public float recoveryInterval = 3f; // ȸ�� ����

    public Text resourceText; // �ڿ� ���� ǥ�ÿ� �ؽ�Ʈ

    private void Start()
    {
        InvokeRepeating(nameof(RecoverResource), recoveryInterval, recoveryInterval);
        UpdateResourceUI(); // �ʱ� �ڿ� UI ������Ʈ
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
}
