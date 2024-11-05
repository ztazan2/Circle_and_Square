using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyPanel; // 로비 패널 (인스펙터에서 참조)
    private PanelManager panelManager; // PanelManager 참조
    public static bool isRestarting = false; // 씬 재시작 여부를 나타내는 변수

    void Start()
    {
        panelManager = PanelManager.Instance;

        // 게임 시작 시 처음만 로비 패널을 활성화
        if (!isRestarting)
        {
            ShowLobby();
            Time.timeScale = 0f; // 로비 패널이 열리면서 게임 일시정지
        }
        else
        {
            isRestarting = false; // 재시작 후에는 로비 패널을 다시 열리지 않도록 설정
            Time.timeScale = 1f; // 씬 로드 시 게임 재개
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
        // 로비를 닫고 재시작 상태를 설정하여 로비가 다시 열리지 않도록 설정
        HideLobby();
        Debug.Log("게임 재시작");
        isRestarting = true; // 재시작 상태로 설정
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬을 다시 로드
    }
}
