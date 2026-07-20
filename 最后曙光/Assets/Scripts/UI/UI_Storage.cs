using Unity.VisualScripting;
using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent storageParent;

    public void SetupStorageUI(Inventory_Storage storage)
    {
        this.storage = storage;
        inventory = storage.playerInventory;
        
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();

        foreach(var slot in storageSlots)
            slot.SetStorage(storage);
    }

    void OnEnable()
    {
        //每次打开时更新一下UI（保险）
        UpdateUI();
        EventCenter.InventoryChangeEvent += UpdateUI;
    }

    void OnDisable()
    {
        EventCenter.InventoryChangeEvent -= UpdateUI;
    }

    private void UpdateUI()
    {
        if(storage == null) return;

        inventoryParent.UpdateSlots(inventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
    }
}
