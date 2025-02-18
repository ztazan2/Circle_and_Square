using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    public GameObject diePanel;    // Die 패널 (메인 메뉴에 표시될)
    public GameObject lobbyPanel;  // 로비 패널 (메인 메뉴에 표시될)
    private PanelManager panelManager; // PanelManager 인스턴스 참조

    void Start()
    {
        // PanelManager 인스턴스를 가져옵니다.
        panelManager = PanelManager.Instance;
    }

    // A 버튼(로비 버튼)이 클릭되었을 때 호출되는 메서드: Die 패널을 닫고 로비 패널을 엽니다.
    public void OnLobbyButtonClicked()
    {
        if (panelManager != null)
        {
            // Die 패널이 열려 있다면 닫습니다.
            if (diePanel != null)
            {
                panelManager.ClosePanel();
            }

            // 로비 패널이 있다면 엽니다.
            if (lobbyPanel != null)
            {
                panelManager.OpenPanel(lobbyPanel);
                Debug.Log("로비 패널이 열렸습니다.");
            }
        }
    }

    // B 버튼(재시작 버튼)이 클릭되었을 때 호출되는 메서드: Die 패널을 닫고 게임을 재시작합니다.
    public void OnRestartButtonClicked()
    {
        if (panelManager != null)
        {
            // Die 패널이 열려 있다면 닫습니다.
            if (diePanel != null)
            {
                panelManager.ClosePanel();
            }

            Debug.Log("게임을 재시작합니다.");

            // 로비 패널 대신 재시작을 위해 재시작 플래그를 설정하고 현재 씬을 재로드합니다.
            LobbyManager.isRestarting = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 재로드
        }
    }
}
