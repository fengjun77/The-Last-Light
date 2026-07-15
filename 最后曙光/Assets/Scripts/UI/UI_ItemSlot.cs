using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;

    [Header("UI格子设置")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    private float doubleClickThreshold = .3f;
    private float lastClickTime;
    private bool waitSecondClick;

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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null) return;

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            float now = Time.unscaledTime;

            if(waitSecondClick && now - lastClickTime < doubleClickThreshold)
            {
                inventory.LearnSkill(itemInSlot);
                waitSecondClick = false;
            }
            else
            {
                inventory.TryEquipItem(itemInSlot);
                lastClickTime = now;
                waitSecondClick = true;
            }
        }
        
    }
}
