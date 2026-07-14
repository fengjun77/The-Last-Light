using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamageable
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>(); 
    }

    public bool TakeDamage(float damage, Transform damageDealer)
    {
        anim.SetBool("openChest", true);

        return true;
    }
}
