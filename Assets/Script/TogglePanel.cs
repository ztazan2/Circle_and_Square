using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanelController : MonoBehaviour
{
    public GameObject Panel_esc; // 열고 닫을 패널
    public Text timeText; // 시간을 표시할 Text 컴포넌트
    private float elapsedTime; // 경과 시간 변수
    private bool isPanelOpen = false; // 패널 상태 확인 변수

    private void Update()
    {
        if (!isPanelOpen) // 패널이 닫혀 있을 때만 시간 업데이트
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    public void TogglePanel()
    {
        if (Panel_esc != null)
        {
            isPanelOpen = !Panel_esc.activeSelf;

            if (isPanelOpen)
            {
                PanelManager.Instance.OpenPanel(Panel_esc);
                Time.timeScale = 0; // 게임 정지
            }
            else
            {
                PanelManager.Instance.ClosePanel(Panel_esc);
                Time.timeScale = 1; // 게임 재개
            }
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // 창이 포커스를 잃었을 때 패널을 자동으로 엽니다.
        if (!hasFocus)
        {
            if (Panel_esc != null && !Panel_esc.activeSelf)
            {
                isPanelOpen = true;
                PanelManager.Instance.OpenPanel(Panel_esc);
                Time.timeScale = 0; // 게임 정지
            }
        }
    }
}
