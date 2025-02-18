using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    // 싱글톤 인스턴스 선언
    public static PanelManager Instance { get; private set; }

    private GameObject currentOpenPanel; // 현재 열려있는 패널

    [SerializeField] private List<GameObject> players;    // 플레이어 오브젝트 목록
    [SerializeField] private List<GameObject> enemies;    // 적 오브젝트 목록
    [SerializeField] private List<GameObject> inspectorPanels; // 인스펙터에서 할당한 패널 목록

    private PlayerSkill playerSkill; // PlayerSkill 컴포넌트 참조

    // 싱글톤 인스턴스 초기화
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // 씬 로드 이벤트 등록
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 씬 로드 이벤트 해제
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때 호출되는 이벤트 핸들러
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에 있는 PlayerSkill 컴포넌트를 다시 찾습니다.
        playerSkill = FindObjectOfType<PlayerSkill>();
    }

    // 인스펙터에 할당된 패널 목록에 포함된 경우에만 패널을 열어 기존처럼 처리
    public bool OpenPanel(GameObject panel)
    {
        // 인스펙터에서 패널이 할당되었다면, 해당 패널이 목록에 포함되어 있는지 확인
        if (inspectorPanels != null && inspectorPanels.Count > 0)
        {
            if (!inspectorPanels.Contains(panel))
            {
                Debug.LogWarning($"{panel.name} 패널은 인스펙터에 할당된 패널 목록에 포함되어 있지 않습니다.");
                return false;
            }
        }

        // 이미 다른 패널이 열려있는 경우
        if (currentOpenPanel != null && currentOpenPanel != panel)
        {
            Debug.Log("다른 패널이 이미 열려 있어서 해당 패널을 열 수 없습니다.");
            return false;
        }

        // 같은 패널이 이미 열려있는 경우
        if (currentOpenPanel == panel)
        {
            Debug.Log("해당 패널은 이미 열려 있습니다.");
            return false;
        }

        // 패널 열기 처리
        currentOpenPanel = panel;
        panel.SetActive(true);
        panel.transform.SetAsLastSibling();
        Debug.Log($"{panel.name} 패널이 열렸습니다.");

        DisableEntities();    // 패널이 열릴 때 플레이어와 적 오브젝트들을 비활성화
        Time.timeScale = 0f;  // 게임 일시정지

        if (playerSkill != null)
        {
            playerSkill.SetButtonInteractable(false); // 스킬 버튼 비활성화
        }

        return true;
    }

    // 현재 열린 패널을 닫고 엔티티들을 다시 활성화하는 메서드
    public void ClosePanel()
    {
        if (currentOpenPanel != null)
        {
            currentOpenPanel.SetActive(false);
            Debug.Log($"{currentOpenPanel.name} 패널이 닫혔습니다.");
            currentOpenPanel = null;

            EnableEntities();   // 플레이어와 적 오브젝트들을 활성화
            Time.timeScale = 1f; // 게임 재개

            if (playerSkill != null)
            {
                playerSkill.SetButtonInteractable(true); // 스킬 버튼 활성화
            }
        }
    }

    // 현재 패널이 열려있는지 여부 반환
    public bool IsPanelOpen()
    {
        return currentOpenPanel != null;
    }

    // 현재 열려 있는 패널 반환
    public GameObject GetCurrentOpenPanel()
    {
        return currentOpenPanel;
    }

    // 플레이어와 적 오브젝트들을 비활성화하는 메서드
    private void DisableEntities()
    {
        SetActiveForEntities(players, false);
        SetActiveForEntities(enemies, false);
    }

    // 플레이어와 적 오브젝트들을 활성화하는 메서드
    private void EnableEntities()
    {
        SetActiveForEntities(players, true);
        SetActiveForEntities(enemies, true);
    }

    // 전달받은 엔티티 리스트의 모든 오브젝트에 대해 활성화 상태 설정
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
