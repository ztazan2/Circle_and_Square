using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    public GameObject[] panels;             // 제어할 패널 배열 (게임 내 UI 패널)
    public PanelManager panelManager;       // Inspector에서 할당할 PanelManager 참조

    private Button resumeButton;            // Resume 버튼의 Button 컴포넌트 참조

    void Start()
    {
        resumeButton = GetComponent<Button>();  // 현재 오브젝트의 Button 컴포넌트 가져오기

        // 인스펙터에 할당되지 않았다면, 싱글톤 인스턴스를 통해 할당
        if (panelManager == null)
        {
            panelManager = PanelManager.Instance;
        }

        UpdateResumeButtonState();              // 초기 버튼 상태 설정
    }

    void Update()
    {
        UpdateResumeButtonState();              // 매 프레임마다 버튼 활성화 여부 갱신
    }

    // Resume 버튼의 활성화 상태를 업데이트하는 메서드
    private void UpdateResumeButtonState()
    {
        bool shouldEnableButton = false;

        if (panelManager != null)
        {
            GameObject currentOpenPanel = panelManager.GetCurrentOpenPanel();

            foreach (GameObject panel in panels)
            {
                if (currentOpenPanel == panel)
                {
                    shouldEnableButton = true;
                    break;
                }
            }
        }

        if (resumeButton != null)
        {
            resumeButton.interactable = shouldEnableButton;
        }
    }

    // 게임을 일시정지하는 메서드
    public void PauseGame()
    {
        Time.timeScale = 0; // 게임 일시정지 (시간 멈춤)
    }

    // 게임을 재개하는 메서드
    public void ResumeGame()
    {
        bool hasAssignedPanelOpen = false;

        if (panelManager != null)
        {
            GameObject currentOpenPanel = panelManager.GetCurrentOpenPanel();

            foreach (GameObject panel in panels)
            {
                if (panel == currentOpenPanel)
                {
                    hasAssignedPanelOpen = true;
                    break;
                }
            }
        }

        if (hasAssignedPanelOpen)
        {
            // 지정된 패널이 열려 있다면 해당 패널을 닫고 게임을 재개
            if (panelManager != null)
            {
                panelManager.ClosePanel();
            }
            Time.timeScale = 1; // 게임 재개 (시간 정상 진행)
        }
        else
        {
            Debug.Log("지정된 패널이 열려 있지 않아 게임을 재개할 수 없습니다.");
        }
    }

    // Resume 버튼 클릭 시 호출되는 메서드
    public void OnResumeButtonClick()
    {
        ResumeGame(); // Resume 버튼 클릭 시 게임 재개 처리
    }
}
