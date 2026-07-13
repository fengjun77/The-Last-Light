using NUnit.Framework;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("反击相关")]
    [SerializeField] private float counterDuration;

    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;
        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if(counterable == null) continue;

            if(counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasPerformedCounter = true;
            }
        }

        return hasPerformedCounter;
    }

    public float GetCounterDuration() => counterDuration;
}
