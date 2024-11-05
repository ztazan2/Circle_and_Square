using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    public GameObject diePanel;    // Die 패널 (인스펙터에서 참조)
    public GameObject lobbyPanel;  // 로비 패널 (인스펙터에서 참조)
    private PanelManager panelManager; // PanelManager 참조

    void Start()
    {
        // PanelManager 인스턴스를 가져옵니다.
        panelManager = PanelManager.Instance;
    }

    // A 버튼이 클릭되었을 때 로비 패널을 여는 메서드
    public void OnLobbyButtonClicked()
    {
        if (panelManager != null)
        {
            Time.timeScale = 1f; // 게임 일시정지 해제

            // Die 패널을 닫고 로비 패널을 연다.
            if (diePanel != null)
            {
                panelManager.ClosePanel(diePanel);
            }

            if (lobbyPanel != null)
            {
                panelManager.OpenPanel(lobbyPanel);
                Debug.Log("로비 패널이 열렸습니다.");
            }
        }
    }

    // B 버튼이 클릭되었을 때 스테이지를 재시작하는 메서드
    public void OnRestartButtonClicked()
    {
        if (panelManager != null)
        {
            Time.timeScale = 1f; // 게임 일시정지 해제

            // Die 패널을 닫고 씬 재시작
            if (diePanel != null)
            {
                panelManager.ClosePanel(diePanel);
            }

            Debug.Log("스테이지 재시작");

            // 로비가 열리지 않도록 설정
            LobbyManager.isRestarting = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬을 다시 로드
        }
    }
}
