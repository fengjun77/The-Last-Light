using UnityEngine;

public class Object_Chest : MonoBehaviour, IInteractable
{
    private Animator anim;

    private Entity_DropManager dropManager;
    private bool canDropItems = true;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>(); 
        dropManager = GetComponent<Entity_DropManager>();
    }

    public void Interact()
    {
        if(canDropItems == false)
            return;

        canDropItems = false;
        dropManager?.DropItems();
        anim.SetBool("openChest", true);

        Destroy(gameObject, 2);       
    }
}
