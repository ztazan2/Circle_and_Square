using UnityEngine;

public class EndGamePanelController : MonoBehaviour
{
    public GameObject endGamePanelA; // 승리 패널
    public GameObject endGamePanelB; // 패배 패널
    private bool gameEnded = false;
    private PanelManager panelManager;

    void Start()
    {
        panelManager = PanelManager.Instance;
    }

    // 게임 종료 메서드
    public void EndGame(bool isPlayerBaseDestroyed)
    {
        if (!gameEnded)
        {
            gameEnded = true;
            GameObject panelToOpen = isPlayerBaseDestroyed ? endGamePanelB : endGamePanelA;

            // 패널을 열고 게임을 일시정지
            if (panelToOpen != null && panelManager != null)
            {
                panelManager.OpenPanel(panelToOpen);
            }
            Time.timeScale = 0f;
        }
    }

    // 게임 재시작 메서드
    public void RestartGame()
    {
        Time.timeScale = 1f; // 일시정지 해제
        gameEnded = false;

        // 모든 패널 닫기
        if (panelManager != null)
        {
            panelManager.ClosePanel(endGamePanelA);
            panelManager.ClosePanel(endGamePanelB);
        }
    }
}
