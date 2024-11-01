using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }
    private GameObject currentOpenPanel;

    [SerializeField] private List<GameObject> players; // 플레이어 오브젝트 리스트 참조
    [SerializeField] private List<GameObject> enemies; // 에너미 오브젝트 리스트 참조

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

    // 패널을 열고, 현재 열려 있는 다른 패널이 있으면 닫는 메서드
    public void OpenPanel(GameObject panel)
    {
        if (currentOpenPanel != null && currentOpenPanel != panel)
        {
            currentOpenPanel.SetActive(false);
        }

        currentOpenPanel = panel;
        panel.SetActive(true);
        panel.transform.SetAsLastSibling(); // 패널을 화면의 맨 앞으로 가져옴

        // 플레이어와 에너미들을 비활성화
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

    // 현재 열려 있는 패널을 닫는 메서드
    public void ClosePanel(GameObject panel)
    {
        if (currentOpenPanel == panel)
        {
            currentOpenPanel.SetActive(false);
            currentOpenPanel = null;

            // 플레이어와 에너미들을 다시 활성화
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
