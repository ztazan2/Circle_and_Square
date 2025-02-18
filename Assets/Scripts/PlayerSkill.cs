using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public Button skillButton; // 스킬 버튼
    public Image cooldownOverlay; // 쿨다운 진행 상태를 표시하는 오버레이 이미지
    public float cooldownTime; // 스킬 쿨다운 시간 (초)
    private float lastSkillTime = -Mathf.Infinity; // 마지막으로 스킬을 사용한 시간 (초기값: 무한히 이전)
    private bool isCooldown = false; // 현재 쿨다운 상태인지 여부
    private bool isCooldownBypassed = false; // 쿨다운 무시 여부 (특정 효과로 즉시 사용 가능)

    public GameObject cannonPrefab; // 스킬로 생성할 캐논 오브젝트 프리팹
    public Vector3 cannonSpawnPosition; // 캐논이 생성될 위치

    private void Start()
    {
        // 스킬 버튼 클릭 시 UseSkill() 실행하도록 이벤트 리스너 추가
        skillButton.onClick.AddListener(UseSkill);
        UpdateButtonAndOverlay(); // 버튼 및 UI 초기 상태 업데이트
    }

    private void Update()
    {
        // 패널이 열려 있는 동안 버튼 비활성화
        if (PanelManager.Instance != null && PanelManager.Instance.IsPanelOpen())
        {
            skillButton.interactable = false;
            cooldownOverlay.fillAmount = 1; // 패널이 열려 있으면 쿨다운 오버레이를 가득 채움
            skillButton.image.color = Color.gray; // 비활성화 상태 색상 적용
            return; // 더 이상 업데이트하지 않음
        }

        // 쿨다운이 진행 중이고, 쿨다운 무시 상태가 아닐 경우
        if (isCooldown && !isCooldownBypassed)
        {
            float elapsedCooldown = Time.time - lastSkillTime; // 경과 시간 계산
            float cooldownRatio = elapsedCooldown / cooldownTime; // 쿨다운 진행률 계산
            cooldownOverlay.fillAmount = Mathf.Clamp01(cooldownRatio); // 0~1 사이로 제한

            // 쿨다운이 끝나면 상태 초기화
            if (cooldownRatio >= 1f)
            {
                isCooldown = false;
                UpdateButtonAndOverlay();
            }
        }
        else
        {
            UpdateButtonAndOverlay(); // 쿨다운이 아닐 경우 UI 업데이트
        }
    }

    // 스킬 사용 메서드
    public void UseSkill()
    {
        if (!isCooldown || isCooldownBypassed) // 쿨다운 상태가 아니거나 쿨다운 무시 상태일 경우
        {
            Debug.Log("스킬 발동!");
            lastSkillTime = Time.time; // 현재 시간을 마지막 스킬 사용 시간으로 저장
            isCooldown = true; // 쿨다운 시작
            isCooldownBypassed = false; // 쿨다운 무시 해제
            UpdateButtonAndOverlay(); // 버튼 및 UI 업데이트

            // 캐논 오브젝트 생성
            Instantiate(cannonPrefab, cannonSpawnPosition, Quaternion.identity);
        }
    }

    // 버튼 및 쿨다운 오버레이 UI를 업데이트하는 메서드
    private void UpdateButtonAndOverlay()
    {
        skillButton.interactable = !isCooldown || isCooldownBypassed; // 쿨다운 상태가 아닐 때 활성화
        cooldownOverlay.fillAmount = skillButton.interactable ? 1 : 0; // 버튼 활성화 여부에 따라 오버레이 조절
        skillButton.image.color = skillButton.interactable ? Color.yellow : Color.gray; // 색상 변경
    }

    // 쿨다운을 무시하는 기능 (특정 효과나 아이템 사용 시 호출 가능)
    public void BypassCooldown()
    {
        isCooldownBypassed = true; // 쿨다운 무시 활성화
        UpdateButtonAndOverlay(); // UI 업데이트
        Debug.Log("쿨다운 무시 활성화!");
    }

    // 쿨다운이 완료되었는지 확인하는 메서드
    public bool IsCooldownComplete()
    {
        return !isCooldown || isCooldownBypassed; // 쿨다운이 끝났거나, 쿨다운 무시 상태이면 true 반환
    }

    // 버튼의 상호작용 가능 여부를 설정하는 메서드
    public void SetButtonInteractable(bool isInteractable)
    {
        skillButton.interactable = isInteractable; // 버튼 활성화 여부 설정
        UpdateButtonAndOverlay(); // UI 업데이트
    }
}
