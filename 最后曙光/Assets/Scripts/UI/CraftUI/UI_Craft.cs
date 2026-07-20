using UnityEngine;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventoryParent;
    private Inventory_Player inventory;
    private UI_CraftPreview craftPreviewUI;
    private UI_CraftSlot[] craftSlots;
    private UI_CraftListButton[] craftListButtons;

    /// <summary>
    /// 设置相关所需的组件
    /// </summary>
    /// <param name="storage"></param>
    public void SetupCraftUI(Inventory_Storage storage)
    {
        inventory = storage.playerInventory;   
        UpdateUI();

        craftPreviewUI = GetComponentInChildren<UI_CraftPreview>();
        craftPreviewUI.SetupCraftPreview(storage); 
        SetupCraftListButtons();
    }

    void OnEnable()
    {
        //每次打开变为初始化状态
        UpdateUI();
        SetupCraftListButtons();
        EventCenter.InventoryChangeEvent += UpdateUI;
    }

    void OnDisable()
    {
        EventCenter.InventoryChangeEvent -= UpdateUI;
    }

    /// <summary>
    /// 隐藏所有格子并初始化各个系列按钮绑定的物品信息
    /// </summary>
    public void SetupCraftListButtons()
    {
        //加true保证每次都能获得所有
        craftSlots = GetComponentsInChildren<UI_CraftSlot>(true);
        craftListButtons = GetComponentsInChildren<UI_CraftListButton>(true);

        foreach(var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        foreach(var button in craftListButtons)
        {
            button.SetCraftSlots(craftSlots);
        }
    }

    private void UpdateUI()
    {
        inventoryParent.UpdateSlots(inventory.itemList);
    }
}
