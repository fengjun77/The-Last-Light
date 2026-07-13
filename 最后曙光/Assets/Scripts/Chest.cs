using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>(); 
    }

    public void TakeDamage(float damage, Transform damageDealer)
    {
        anim.SetBool("openChest", true);
    }
}
