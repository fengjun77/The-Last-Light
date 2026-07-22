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
        EventCenter.HealthChangeEvent += UpdateHealthBarUI;
        EventCenter.FlipEvent += HandleFlip;
    }

    void OnDisable()
    {
        EventCenter.HealthChangeEvent -= UpdateHealthBarUI;
        EventCenter.FlipEvent -= HandleFlip;
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
