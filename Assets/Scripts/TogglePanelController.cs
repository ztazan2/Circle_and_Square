using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TogglePanelController : MonoBehaviour
{
    public GameObject Panel_esc;    // ESC 메뉴 패널 (일시정지 화면)
    public Text timeText;           // 플레이 시간 표시 UI 텍스트

    private float elapsedTime;      // 게임 플레이 경과 시간
    private bool isPanelOpen = false; // 현재 패널이 열려 있는지 여부

    private void Start()
    {
        // 초기화 작업이 필요하면 여기서 진행합니다.
    }

    private void Update()
    {
        // 패널이 열려 있지 않을 때만 경과 시간을 계속 증가시킴
        if (!isPanelOpen)
        {
            // Time.timeScale의 영향을 받지 않음
            elapsedTime += Time.unscaledDeltaTime;

            // 시간 UI 업데이트
            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    // ESC 패널을 토글하는 메서드
    public void TogglePanel()
    {
        if (Panel_esc != null)
        {
            // 현재 패널 상태를 반전
            isPanelOpen = !Panel_esc.activeSelf;

            if (isPanelOpen)
            {
                // ESC 패널 열기 및 게임 일시정지
                PanelManager.Instance.OpenPanel(Panel_esc);
                Time.timeScale = 0; // 게임 정지
            }
            else
            {
                // ESC 패널 닫기 및 게임 재개
                PanelManager.Instance.ClosePanel();
                Time.timeScale = 1; // 게임 정상 진행
            }
        }
    }
}
