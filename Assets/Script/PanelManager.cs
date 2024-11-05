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

    private void Start()
    {
        // 씬이 재시작된 상태라면 유닛을 비활성화하고, 패널이 열렸을 때에만 유닛이 활성화되도록 설정
        if (LobbyManager.isRestarting)
        {
            DisableEntities();
            Time.timeScale = 0f;
            LobbyManager.isRestarting = false; // 재시작 상태 초기화
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

        // 유닛을 비활성화하고 게임을 일시정지
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

            // 유닛 활성화 및 게임 재개
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
