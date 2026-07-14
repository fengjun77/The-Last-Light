using UnityEngine;
using UnityEngine.UI;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Slider healthBar;
    private Entity owner;

    void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
        owner = GetComponentInParent<Entity>();
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

    private void UpdateHealthBarUI(float currentHp, float maxHP, Entity entity)
    {
        if(entity != owner)
            return;

        healthBar.value = currentHp / maxHP;
    }

    private void HandleFlip()
    {
        transform.rotation = Quaternion.identity;
    }
}
