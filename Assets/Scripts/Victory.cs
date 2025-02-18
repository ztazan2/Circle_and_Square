using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject victoryPanel; // 승리 패널 (메인 메뉴에 표시될)
    public GameObject lobbyPanel;   // 로비 패널 (메인 메뉴에 표시될)
    private PanelManager panelManager; // PanelManager 인스턴스 참조

    void Start()
    {
        // PanelManager 인스턴스를 가져옵니다.
        panelManager = PanelManager.Instance;
    }

    // 승리 버튼이 클릭되었을 때 호출되는 메서드
    public void OnVictoryButtonClicked()
    {
        if (panelManager != null)
        {
            // 승리 패널이 열려 있다면 닫고 로비 패널을 엽니다.
            if (victoryPanel != null)
            {
                panelManager.ClosePanel();
            }

            if (lobbyPanel != null)
            {
                panelManager.OpenPanel(lobbyPanel);
                Debug.Log("로비 패널이 열렸습니다.");
            }
        }
    }
}
