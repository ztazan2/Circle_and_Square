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

            // ���� �г��� ���� ������ ���� ��Ȱ��ȭ
            if (currentOpenPanel != null)
            {
                SetActiveForEntities(players, false);
                SetActiveForEntities(enemies, false);
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            Destroy(gameObject);
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

        SetActiveForEntities(players, false);
        SetActiveForEntities(enemies, false);
        Time.timeScale = 0f;

        return true;
    }

    public void ClosePanel(GameObject panel)
    {
        if (currentOpenPanel == panel)
        {
            panel.SetActive(false);
            currentOpenPanel = null;

            SetActiveForEntities(players, true);
            SetActiveForEntities(enemies, true);
            Time.timeScale = 1f;
        }
    }

    public bool IsPanelOpen()
    {
        return currentOpenPanel != null;
    }

    private void SetActiveForEntities(List<GameObject> entities, bool isActive)
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
