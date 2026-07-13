using UnityEngine;
using UnityEngine.UI;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Slider healthBar;

    void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
    }

    void OnEnable()
    {
        EventCenter.OnHealthChange += UpdateHealthBarUI;
        EventCenter.OnFlip += HandleFlip;
    }

    void OnDisable()
    {
        EventCenter.OnHealthChange -= UpdateHealthBarUI;
        EventCenter.OnFlip -= HandleFlip;
    }

    private void UpdateHealthBarUI(float currentHp, float maxHP)
    {
        healthBar.value = currentHp / maxHP;
    }

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
