using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject[] panels; // 제어할 패널 배열로 참조

    void Start()
    {
        ResumeGame(); // 게임이 시작될 때 자동으로 실행 상태로 설정
    }

    public void PauseGame()
    {
        // 게임을 일시 정지시키고, 패널을 열지 않음
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        // PanelManager를 통해 할당된 모든 패널 닫기
        foreach (GameObject panel in panels)
        {
            PanelManager.Instance.ClosePanel(panel);
        }
        Time.timeScale = 1; // 게임을 다시 시작
    }

    public void OnResumeButtonClick()
    {
        // Resume 버튼 클릭 시 할당된 패널을 모두 닫고 게임을 재개
        ResumeGame();
    }
}
