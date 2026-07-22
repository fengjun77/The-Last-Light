using UnityEngine;

public class Player_Health : Entity_Health
{
    private Player player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            Die();
    }

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

    protected override void Die()
    {
        base.Die();

        player.ui.OpenDeathScreenUI();
    }
}
