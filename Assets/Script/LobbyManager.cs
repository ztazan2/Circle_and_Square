using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyPanel; // �κ� �г� (�ν����Ϳ��� ����)
    private PanelManager panelManager; // PanelManager ����
    private static bool isRestarting = false; // �� ����� ���θ� ��Ÿ���� ����

    void Start()
    {
        panelManager = PanelManager.Instance;

        if (!isRestarting)
        {
            ShowLobby();
            Time.timeScale = 0f; // �κ� �г��� �����鼭 ���� �Ͻ�����
        }
        else
        {
            isRestarting = false; // ���� �ε忡�� �κ� �г��� �ٽ� ������ �ʱ�ȭ
            Time.timeScale = 1f; // �� �ε� �� �簳
        }
    }

    public void ShowLobby()
    {
        if (lobbyPanel != null && panelManager != null)
        {
            if (panelManager.OpenPanel(lobbyPanel))
            {
                Debug.Log("�κ� �г��� ���Ƚ��ϴ�.");
                Time.timeScale = 0f; // �κ� �г� ����� ���ÿ� ���� �Ͻ�����
            }
        }
    }

    public void HideLobby()
    {
        if (lobbyPanel != null && panelManager != null)
        {
            panelManager.ClosePanel(lobbyPanel);
            Debug.Log("�κ� �г��� �������ϴ�.");
            Time.timeScale = 1f; // �κ� �г� �ݱ�� ���ÿ� ���� �簳
        }
    }

    public void StartGame()
    {
        HideLobby();
        Debug.Log("������ ���۵˴ϴ�.");
    }

    public void RestartGame()
    {
        HideLobby();
        Debug.Log("���� �����");
        isRestarting = true; // ����� ���·� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� ���� �ٽ� �ε�
    }
}
