using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemDataSO itemData;

    private Inventory_Item itemToAdd;
    private Inventory_Base inventory;

    void Awake()
    {
        itemToAdd = new Inventory_Item(itemData);
    }

    void OnValidate()
    {
        if(itemData == null) return;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickup - " + itemData.itemName;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        inventory = collision.GetComponent<Inventory_Base>();

        bool canAddItem = inventory.CanAddItem() || inventory.StackableItem(itemToAdd) != null;
        
        if(inventory != null && canAddItem)
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        } 
    }
}
