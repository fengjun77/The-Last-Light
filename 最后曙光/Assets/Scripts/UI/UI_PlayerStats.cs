using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatSlot[] uiStatSlots;

    private void Awake()
    {
        uiStatSlots = GetComponentsInChildren<UI_StatSlot>();
    }

    void OnEnable()
    {
        EventCenter.InventoryChangeEvent += UpdateStatsUI;
    }

    void OnDisable()
    {
        EventCenter.InventoryChangeEvent -= UpdateStatsUI;
    }

    void Start()
    {
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach(var slot in uiStatSlots)
        {
            slot.UpdateStatValue();
        }
    }
}
