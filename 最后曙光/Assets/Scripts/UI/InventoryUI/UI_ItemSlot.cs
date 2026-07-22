using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected RectTransform rect;
    protected UI ui;

    public Inventory_Item itemInSlot { get; private set; }
    protected Inventory_Player inventory;

    [Header("UI格子设置")]
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemStackSize;

    private float doubleClickThreshold = .3f;
    private float lastClickTime;
    private bool waitSecondClick;

    void Awake()
    {
        inventory = FindAnyObjectByType<Inventory_Player>();
        ui = FindAnyObjectByType<UI>();
        rect = GetComponent<RectTransform>();
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
        if(itemInSlot == null) return;//|| itemInSlot.itemData.itemType == ItemType.Material

        bool rightButton = eventData.button == PointerEventData.InputButton.Right;
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;

        if(leftButton)
        {
            float now = Time.unscaledTime;

            if(waitSecondClick && now - lastClickTime < doubleClickThreshold)
            {
                if(itemInSlot.itemData.itemType == ItemType.Consumable)
                {
                    inventory.TryUseItem(itemInSlot);
                }
                else if(itemInSlot.itemData.itemType == ItemType.SkillScroll)
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
        else if(rightButton)
        {
            bool deleteItem = Input.GetKey(KeyCode.LeftAlt);
            if(deleteItem)
            {
                inventory.RemoveFullStack(itemInSlot);
                Debug.Log("成功丢弃");
            }
        }

        if(itemInSlot == null)
            EventCenter.OnShowItemToolTipEvent(false, rect, null,false);   
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(itemInSlot == null) return;

        EventCenter.OnShowItemToolTipEvent(true, rect, itemInSlot, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventCenter.OnShowItemToolTipEvent(false, rect, null, false);
    }
}
