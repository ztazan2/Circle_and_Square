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

            // 현재 패널이 열려 있으면 유닛 비활성화
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
            Debug.Log("다른 패널이 이미 열려 있어 이 패널을 열 수 없습니다.");
            return false;
        }

        if (currentOpenPanel == panel)
        {
            Debug.Log("같은 패널이 이미 열려 있습니다.");
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
