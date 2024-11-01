using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanelController : MonoBehaviour
{
    public GameObject Panel_esc; // ���� ���� �г�
    public Text timeText; // �ð��� ǥ���� Text ������Ʈ
    private float elapsedTime; // ��� �ð� ����
    private bool isPanelOpen = false; // �г� ���� Ȯ�� ����

    private void Update()
    {
        if (!isPanelOpen) // �г��� ���� ���� ���� �ð� ������Ʈ
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
                Time.timeScale = 0; // ���� ����
            }
            else
            {
                PanelManager.Instance.ClosePanel(Panel_esc);
                Time.timeScale = 1; // ���� �簳
            }
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // â�� ��Ŀ���� �Ҿ��� �� �г��� �ڵ����� ���ϴ�.
        if (!hasFocus)
        {
            if (Panel_esc != null && !Panel_esc.activeSelf)
            {
                isPanelOpen = true;
                PanelManager.Instance.OpenPanel(Panel_esc);
                Time.timeScale = 0; // ���� ����
            }
        }
    }
}
