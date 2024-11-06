using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int maxResource = 1000;  // 최대 자원량
    public int currentResource = 0; // 현재 자원량
    public int resourceRecoveryRate = 100; // 자원 회복량
    public float recoveryInterval = 3f; // 회복 간격

    public Text resourceText; // 자원 상태 표시용 텍스트

    private void Start()
    {
        InvokeRepeating(nameof(RecoverResource), recoveryInterval, recoveryInterval);
        UpdateResourceUI(); // 초기 자원 UI 업데이트
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
