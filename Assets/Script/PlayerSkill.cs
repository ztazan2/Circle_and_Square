using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public Button skillButton;
    public Image cooldownOverlay;
    public float cooldownTime = 5f;
    private float lastSkillTime = -Mathf.Infinity;
    private bool isCooldown = false;

    public GameObject cannonPrefab;
    public Vector3 cannonSpawnPosition;

    private void Start()
    {
        skillButton.onClick.AddListener(UseSkill);
        UpdateButtonColor();
    }

    private void Update()
    {
        if (isCooldown)
        {
            float elapsedCooldown = Time.time - lastSkillTime;
            float cooldownRatio = elapsedCooldown / cooldownTime;
            cooldownOverlay.fillAmount = Mathf.Clamp01(cooldownRatio);

            if (cooldownRatio >= 1f)
            {
                isCooldown = false;
                skillButton.interactable = true;
                cooldownOverlay.fillAmount = 1;
                UpdateButtonColor();
            }
        }
    }

    public void UseSkill()
    {
        if (!isCooldown)
        {
            Debug.Log("스킬 사용!");

            lastSkillTime = Time.time;
            isCooldown = true;
            skillButton.interactable = false;
            UpdateButtonColor();
            cooldownOverlay.fillAmount = 0;

            Instantiate(cannonPrefab, cannonSpawnPosition, Quaternion.identity);
        }
    }

    private void UpdateButtonColor()
    {
        skillButton.image.color = skillButton.interactable ? Color.yellow : Color.gray;
    }
}
