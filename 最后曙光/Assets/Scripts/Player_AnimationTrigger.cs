using UnityEngine;

public class Player_AnimationTrigger : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();    
    }


    public void CurrentStateTrigger()
    {
        player.CallAnimationTrigger();
    }

    public void AttackTrigger()
    {
        
    }
}
