using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private Inventory_Merchant merchant;

    public enum MerchantSlotType { MerchantSlot, PlayerSlot }
    public MerchantSlotType slotType;
    public void SetupMerchantUI(Inventory_Merchant merchant) => this.merchant = merchant;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null) return;

        bool rightButton = eventData.button == PointerEventData.InputButton.Right;
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;

        //如果是玩家背包格子
        if(slotType == MerchantSlotType.PlayerSlot)
        {
            //右键出售
            if(rightButton)
            {
                bool sellFullStack = Input.GetKey(KeyCode.LeftControl);

                merchant.TrySellItem(itemInSlot, sellFullStack);
            }
            else if(leftButton)
            {
                base.OnPointerDown(eventData);
            }
        }
        else if(slotType == MerchantSlotType.MerchantSlot)
        {
            if(leftButton)
                return;
            
            //购买物品
            bool buyFullStack = Input.GetKey(KeyCode.LeftControl);
            merchant.TryBuyItem(itemInSlot, buyFullStack);
        }

        EventCenter.OnShowItemToolTipEvent(false, rect, null, false);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(itemInSlot == null) return;

        if(slotType == MerchantSlotType.MerchantSlot)
            EventCenter.OnShowItemToolTipEvent(true, rect, itemInSlot, true);
        else if(slotType == MerchantSlotType.PlayerSlot)
            EventCenter.OnShowItemToolTipEvent(true, rect, itemInSlot, false);
    }
}
