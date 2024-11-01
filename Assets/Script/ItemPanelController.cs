using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelController : MonoBehaviour
{
    public GameObject Panel_item; // 열고 닫을 패널

    void Start()
    {
        // Start 함수 내용은 빈 상태로 둡니다.
    }

    public void TogglePanel()
    {
        if (Panel_item != null)
        {
            bool isActive = !Panel_item.activeSelf;

            if (isActive)
            {
                // PanelManager를 통해 패널을 열고, 다른 패널은 닫습니다.
                PanelManager.Instance.OpenPanel(Panel_item);
                Time.timeScale = 0; // 게임 정지
            }
            else
            {
                // PanelManager를 통해 패널을 닫습니다.
                PanelManager.Instance.ClosePanel(Panel_item);
                Time.timeScale = 1; // 게임 재개
            }
        }
    }
}
