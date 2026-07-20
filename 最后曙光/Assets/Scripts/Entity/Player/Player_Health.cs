using UnityEngine;

public class Player_Health : Entity_Health
{
    void OnEnable()
    {
        EventCenter.IncreaseHealthEvent += IncreaseHealth;
    }

    void OnDisable()
    {
        EventCenter.IncreaseHealthEvent += IncreaseHealth;
    }

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool wasHit = base.TakeDamage(damage, damageDealer);

        if(wasHit)
            EventCenter.OnTakingDamageEvent();

        return wasHit;
    }
}
