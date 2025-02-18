using UnityEngine;

public class EedPanel : MonoBehaviour
{
    public GameObject endGamePanelA; // 승리 패널
    public GameObject endGamePanelB; // 패배 패널
    private bool gameEnded = false; // 게임 종료 여부 플래그
    private PanelManager panelManager; 

    void Start()
    {
        panelManager = PanelManager.Instance;
    }

    // 게임 종료를 처리하는 메서드
    // isPlayerBaseDestroyed가 true이면 플레이어 기지가 파괴되어 패배, false이면 승리 처리
    public void EndGame(bool isPlayerBaseDestroyed)
    {
        if (!gameEnded)
        {
            gameEnded = true;
            GameObject panelToOpen = isPlayerBaseDestroyed ? endGamePanelB : endGamePanelA;

            // PanelManager를 통해 해당 패널을 열어 게임 종료 화면을 표시
            if (panelToOpen != null && panelManager != null)
            {
                panelManager.OpenPanel(panelToOpen);
            }
        }
    }

    // 게임을 재시작할 때 호출되는 메서드
    public void RestartGame()
    {
        gameEnded = false;

        // PanelManager를 통해 현재 열린 패널을 닫음
        if (panelManager != null)
        {
            PanelManager.Instance.ClosePanel();
        }
    }
}
