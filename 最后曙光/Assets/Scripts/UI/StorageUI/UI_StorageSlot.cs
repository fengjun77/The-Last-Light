using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private Inventory_Storage storage;

    public enum StorageSlotType {StorageSlot, playerInventorySlot};
    public bool canInteract = true;

    public StorageSlotType slotType; 

    public void SetStorage(Inventory_Storage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(!canInteract) return;

        if(itemInSlot == null) return;

        bool transferFullStack = Input.GetKey(KeyCode.LeftControl);

        if(slotType == StorageSlotType.StorageSlot)
        {
            storage.FromStorageToPlayer(itemInSlot, transferFullStack);
        }

        if(slotType == StorageSlotType.playerInventorySlot)
        {
            storage.FromPlayerToStorage(itemInSlot, transferFullStack);
        }

        EventCenter.OnShowItemToolTipEvent(false, rect, null, false);
    }
}
