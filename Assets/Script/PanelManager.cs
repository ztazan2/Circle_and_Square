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

    // �г��� ����, ���� ���� �ִ� �ٸ� �г��� ������ �ݴ� �޼���
    public void OpenPanel(GameObject panel)
    {
        if (currentOpenPanel != null && currentOpenPanel != panel)
        {
            currentOpenPanel.SetActive(false);
        }

        currentOpenPanel = panel;
        panel.SetActive(true);
        panel.transform.SetAsLastSibling(); // �г��� ȭ���� �� ������ ������

        // �÷��̾�� ���ʹ̵��� ��Ȱ��ȭ
        foreach (var player in players)
        {
            if (player != null)
            {
                player.SetActive(false);
            }
        }

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }
    }

    // ���� ���� �ִ� �г��� �ݴ� �޼���
    public void ClosePanel(GameObject panel)
    {
        if (currentOpenPanel == panel)
        {
            currentOpenPanel.SetActive(false);
            currentOpenPanel = null;

            // �÷��̾�� ���ʹ̵��� �ٽ� Ȱ��ȭ
            foreach (var player in players)
            {
                if (player != null)
                {
                    player.SetActive(true);
                }
            }

            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.SetActive(true);
                }
            }
        }
    }
}
