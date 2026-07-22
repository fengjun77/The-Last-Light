using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlot : UI_ItemSlot
{
    [SerializeField] private Sprite defaultSprite;

    public void SetupQuickSlotItem(Inventory_Item itemToPass)
    {
        inventory.SetQuickItemInSlot(itemToPass);
    }

    public void UpdateQuickSlotUI(Inventory_Item currentItemInSlot)
    {
        if(currentItemInSlot == null || currentItemInSlot.itemData == null)
        {
            itemIcon.sprite = defaultSprite;
            itemStackSize.text = "";
            return;
        }

        itemIcon.sprite = currentItemInSlot.itemData.itemIcon;
        itemStackSize.text = currentItemInSlot.stackSize.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.inGameUI.OpenQuickItemOptions(this, rect);
    }
}
