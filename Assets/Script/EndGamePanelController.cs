using UnityEngine;

public class EndGamePanelController : MonoBehaviour
{
    public GameObject endGamePanelA; // �¸� �г�
    public GameObject endGamePanelB; // �й� �г�
    private bool gameEnded = false;
    private PanelManager panelManager;

    void Start()
    {
        panelManager = PanelManager.Instance;
    }

    // ���� ���� �޼���
    public void EndGame(bool isPlayerBaseDestroyed)
    {
        if (!gameEnded)
        {
            gameEnded = true;
            GameObject panelToOpen = isPlayerBaseDestroyed ? endGamePanelB : endGamePanelA;

            // �г��� ���� ������ �Ͻ�����
            if (panelToOpen != null && panelManager != null)
            {
                panelManager.OpenPanel(panelToOpen);
            }
            Time.timeScale = 0f;
        }
    }

    // ���� ����� �޼���
    public void RestartGame()
    {
        Time.timeScale = 1f; // �Ͻ����� ����
        gameEnded = false;

        // ��� �г� �ݱ�
        if (panelManager != null)
        {
            panelManager.ClosePanel(endGamePanelA);
            panelManager.ClosePanel(endGamePanelB);
        }
    }
}
