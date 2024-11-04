using UnityEngine;

public class Victory : MonoBehaviour
{
    public GameObject victoryPanel; // �¸� �г� (�ν����Ϳ��� ����)
    public GameObject lobbyPanel;   // �κ� �г� (�ν����Ϳ��� ����)
    private PanelManager panelManager; // PanelManager ����

    void Start()
    {
        // PanelManager �ν��Ͻ��� �����ɴϴ�.
        panelManager = PanelManager.Instance;
    }

    // A ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���
    public void OnVictoryButtonClicked()
    {
        if (panelManager != null)
        {
            // ���� �Ͻ����� ���� (�ʿ��� ���)
            Time.timeScale = 1f;

            // �¸� �г��� �ݰ� �κ� �г��� ����.
            if (victoryPanel != null)
            {
                panelManager.ClosePanel(victoryPanel);
            }

            if (lobbyPanel != null)
            {
                panelManager.OpenPanel(lobbyPanel);
                Debug.Log("�κ� �г��� ���Ƚ��ϴ�.");
            }
        }
    }
}