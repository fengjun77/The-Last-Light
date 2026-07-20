using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Player player;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI hpText;

    void Awake()
    {
        player = FindFirstObjectByType<Player>();
    }

    void OnEnable()
    {
        EventCenter.HealthChangeEvent += UpdateHealthBarUI;
    }

    void OnDisable()
    {
        EventCenter.HealthChangeEvent -= UpdateHealthBarUI;
    }

    private void UpdateHealthBarUI(float currentHp, float maxHp, Entity entity)
    {
        if(entity != player) return;

        slider.value = currentHp / maxHp;
        hpText.text = (int)currentHp + "/" + maxHp; 
    }
}
