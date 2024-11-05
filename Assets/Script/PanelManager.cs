using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }
    private GameObject currentOpenPanel;

    [SerializeField] private List<GameObject> players; // �÷��̾� ������Ʈ ����Ʈ ����
    [SerializeField] private List<GameObject> enemies; // ���ʹ� ������Ʈ ����Ʈ ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ���� ����۵� ���¶�� ������ ��Ȱ��ȭ�ϰ�, �г��� ������ ������ ������ Ȱ��ȭ�ǵ��� ����
        if (LobbyManager.isRestarting)
        {
            DisableEntities();
            Time.timeScale = 0f;
            LobbyManager.isRestarting = false; // ����� ���� �ʱ�ȭ
        }
    }

    public bool OpenPanel(GameObject panel)
    {
        if (currentOpenPanel != null && currentOpenPanel != panel)
        {
            Debug.Log("�ٸ� �г��� �̹� ���� �־� �� �г��� �� �� �����ϴ�.");
            return false;
        }

        if (currentOpenPanel == panel)
        {
            Debug.Log("���� �г��� �̹� ���� �ֽ��ϴ�.");
            return false;
        }

        currentOpenPanel = panel;
        panel.SetActive(true);
        panel.transform.SetAsLastSibling();

        // ������ ��Ȱ��ȭ�ϰ� ������ �Ͻ�����
        DisableEntities();
        Time.timeScale = 0f;

        return true;
    }

    public void ClosePanel(GameObject panel)
    {
        if (currentOpenPanel == panel)
        {
            panel.SetActive(false);
            currentOpenPanel = null;

            // ���� Ȱ��ȭ �� ���� �簳
            EnableEntities();
            Time.timeScale = 1f;
        }
    }

    public bool IsPanelOpen()
    {
        return currentOpenPanel != null;
    }

    public void DisableEntities()
    {
        SetActiveForEntities(players, false);
        SetActiveForEntities(enemies, false);
    }

    public void EnableEntities()
    {
        SetActiveForEntities(players, true);
        SetActiveForEntities(enemies, true);
    }

    public void SetActiveForEntities(List<GameObject> entities, bool isActive)
    {
        foreach (var entity in entities)
        {
            if (entity != null)
            {
                entity.SetActive(isActive);
            }
        }
    }
}
