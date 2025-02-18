using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelController : MonoBehaviour
{
    public GameObject Panel_item; 

    public void TogglePanel()
    {
        if (Panel_item != null)
        {
            bool isActive = !Panel_item.activeSelf;

            if (isActive)
            {
                // PanelManager를 통해 해당 패널을 열고, 다른 패널은 닫습니다.
                PanelManager.Instance.OpenPanel(Panel_item);
                // 게임 일시정지
                Time.timeScale = 0; 
            }
            else
            {
                // PanelManager를 통해 해당 패널을 닫습니다.
                PanelManager.Instance.ClosePanel();
                // 게임 재개
                Time.timeScale = 1; 
            }
        }
    }
}
