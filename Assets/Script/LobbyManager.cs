using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyPanel; // 로비 패널 (인스펙터에서 참조)
    private PanelManager panelManager; // PanelManager 참조
    private static bool isRestarting = false; // 씬 재시작 여부를 나타내는 변수

    void Start()
    {
        panelManager = PanelManager.Instance;

        if (!isRestarting)
        {
            ShowLobby();
            Time.timeScale = 0f; // 로비 패널이 열리면서 게임 일시정지
        }
        else
        {
            isRestarting = false; // 다음 로드에서 로비 패널을 다시 열도록 초기화
            Time.timeScale = 1f; // 씬 로드 시 재개
        }
    }

    public void ShowLobby()
    {
        if (lobbyPanel != null && panelManager != null)
        {
            if (panelManager.OpenPanel(lobbyPanel))
            {
                Debug.Log("로비 패널이 열렸습니다.");
                Time.timeScale = 0f; // 로비 패널 열기와 동시에 게임 일시정지
            }
        }
    }

    public void HideLobby()
    {
        if (lobbyPanel != null && panelManager != null)
        {
            panelManager.ClosePanel(lobbyPanel);
            Debug.Log("로비 패널이 닫혔습니다.");
            Time.timeScale = 1f; // 로비 패널 닫기와 동시에 게임 재개
        }
    }

    public void StartGame()
    {
        HideLobby();
        Debug.Log("게임이 시작됩니다.");
    }

    public void RestartGame()
    {
        HideLobby();
        Debug.Log("게임 재시작");
        isRestarting = true; // 재시작 상태로 설정
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬을 다시 로드
    }
}
