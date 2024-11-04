using UnityEngine;
using UnityEngine.SceneManagement;

public class Die : MonoBehaviour
{
    public GameObject diePanel;    // Die �г� (�ν����Ϳ��� ����)
    public GameObject lobbyPanel;  // �κ� �г� (�ν����Ϳ��� ����)
    private PanelManager panelManager; // PanelManager ����

    void Start()
    {
        // PanelManager �ν��Ͻ��� �����ɴϴ�.
        panelManager = PanelManager.Instance;
    }

    // A ��ư�� Ŭ���Ǿ��� �� �κ� �г��� ���� �޼���
    public void OnLobbyButtonClicked()
    {
        if (panelManager != null)
        {
            Time.timeScale = 1f; // ���� �Ͻ����� ����

            // Die �г��� �ݰ� �κ� �г��� ����.
            if (diePanel != null)
            {
                panelManager.ClosePanel(diePanel);
            }

            if (lobbyPanel != null)
            {
                panelManager.OpenPanel(lobbyPanel);
                Debug.Log("�κ� �г��� ���Ƚ��ϴ�.");
            }
        }
    }

    // B ��ư�� Ŭ���Ǿ��� �� ���������� ������ϴ� �޼���
    public void OnRestartButtonClicked()
    {
        if (panelManager != null)
        {
            Time.timeScale = 1f; // ���� �Ͻ����� ����

            // Die �г��� �ݰ� �� �����
            if (diePanel != null)
            {
                panelManager.ClosePanel(diePanel);
            }

            Debug.Log("�������� �����");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� ���� �ٽ� �ε�
        }
    }
}