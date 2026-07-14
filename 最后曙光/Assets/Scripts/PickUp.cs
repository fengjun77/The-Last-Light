using UnityEngine;

public class PickUp : MonoBehaviour
{
    public CollectibleSO collectible;


    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if(player != null)
        {
            collectible.Collect(player);
            Destroy(gameObject);
        }
    }
}
