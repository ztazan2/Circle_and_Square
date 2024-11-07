using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }
    private GameObject currentOpenPanel;

    [SerializeField] private List<GameObject> players; // 플레이어 오브젝트 리스트 참조
    [SerializeField] private List<GameObject> enemies; // 에너미 오브젝트 리스트 참조
    [SerializeField] private List<Button> externalButtons; // 패널에 속하지 않은 버튼들

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
        if (LobbyManager.isRestarting)
        {
            DisableEntities();
            DisableExternalButtons(); // 로비가 시작될 때 외부 버튼 비활성화
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

        DisableExternalButtons(); // 패널이 열릴 때 외부 버튼 비활성화

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

            EnableExternalButtons(); // 패널이 닫힐 때 외부 버튼 활성화

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

    private void DisableExternalButtons()
    {
        foreach (Button button in externalButtons)
        {
            if (button != null)
            {
                button.interactable = false; // 외부 버튼 비활성화
            }
        }
    }

    private void EnableExternalButtons()
    {
        foreach (Button button in externalButtons)
        {
            if (button != null)
            {
                button.interactable = true; // 외부 버튼 활성화
            }
        }
    }
}
