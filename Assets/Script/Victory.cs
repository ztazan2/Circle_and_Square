using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject victoryPanel; // 승리 패널 (인스펙터에서 참조)
    public GameObject lobbyPanel;   // 로비 패널 (인스펙터에서 참조)
    private PanelManager panelManager; // PanelManager 참조

    void Start()
    {
        // PanelManager 인스턴스를 가져옵니다.
        panelManager = PanelManager.Instance;
    }

    // A 버튼이 클릭되었을 때 호출되는 메서드
    public void OnVictoryButtonClicked()
    {
        if (panelManager != null)
        {
            // 승리 패널을 닫고 로비 패널을 연다.
            if (victoryPanel != null)
            {
                panelManager.ClosePanel(victoryPanel);
            }

            if (lobbyPanel != null)
            {
                panelManager.OpenPanel(lobbyPanel);
                Debug.Log("로비 패널이 열렸습니다.");
            }
        }
    }
}
