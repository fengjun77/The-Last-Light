using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    private Inventory_Player inventory;

    [Header("UI格子设置")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();
    }

    public void UpdateSlot(Inventory_Item item)
    {
        itemInSlot = item;

        if(itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white;
        color.a = .9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = item.stackSize == 1 ? "" : item.stackSize.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null) return;
        inventory.TryEquipItem(itemInSlot);
    }
}
