using System.Collections.Generic;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    private Player player;
    protected Inventory_Player inventory;

    [Header("快捷栏")]
    [SerializeField] private float yOffsetQuickItemParent = 150;
    [SerializeField] private Transform quickItemOptionsParent;
    private UI_QuickItemSlotOption[] quickItemOptions;
    private UI_QuickItemSlot quickItemSlot;
    
    void Start()
    {
        quickItemSlot = GetComponentInChildren<UI_QuickItemSlot>();

        player = FindFirstObjectByType<Player>();

        inventory = player.inventory;
    }

    void OnEnable()
    {
        EventCenter.InventoryChangeEvent += UpdateQuickSlotUI;
    }

    void OnDisable()
    {
        EventCenter.InventoryChangeEvent -= UpdateQuickSlotUI;
    }

    public void UpdateQuickSlotUI()
    {
        if(inventory == null) return;

        Inventory_Item quickItem = inventory.quickItem; 

        quickItemSlot.UpdateQuickSlotUI(quickItem);
    }

    public void OpenQuickItemOptions(UI_QuickItemSlot quickItemSlot, RectTransform targetRect)
    {
        if(quickItemOptions == null)
            quickItemOptions = quickItemOptionsParent.GetComponentsInChildren<UI_QuickItemSlotOption>(true);
        
        if(inventory == null)
        {
            Debug.Log("没找到Inventory");
        }
        List<Inventory_Item> consumables = inventory.itemList.FindAll(item => item.itemData.itemType == ItemType.Consumable);

        for(int i = 0; i < quickItemOptions.Length; i++)
        {
            if(i < consumables.Count)
            {
                quickItemOptions[i].gameObject.SetActive(true);
                quickItemOptions[i].SetupOption(quickItemSlot, consumables[i]);
            }
            else
                quickItemOptions[i].gameObject.SetActive(false);
        }

        quickItemOptionsParent.position = targetRect.position + Vector3.up * yOffsetQuickItemParent;
    }

    public void HideQuickItemOptions() => quickItemOptionsParent.position = new Vector3(0,9999);
}
