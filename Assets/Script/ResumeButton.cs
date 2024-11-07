using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject[] panels; // ������ �г� �迭�� ����

    void Start()
    {
        ResumeGame(); // ������ ���۵� �� �ڵ����� ���� ���·� ����
    }

    public void PauseGame()
    {
        // ������ �Ͻ� ������Ű��, �г��� ���� ����
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        // PanelManager�� ���� �Ҵ�� ��� �г� �ݱ�
        foreach (GameObject panel in panels)
        {
            PanelManager.Instance.ClosePanel(panel);
        }
        Time.timeScale = 1; // ������ �ٽ� ����
    }

    public void OnResumeButtonClick()
    {
        // Resume ��ư Ŭ�� �� �Ҵ�� �г��� ��� �ݰ� ������ �簳
        ResumeGame();
    }
}
