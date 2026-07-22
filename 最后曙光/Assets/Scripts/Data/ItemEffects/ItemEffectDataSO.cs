using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class ItemEffectDataSO : ScriptableObject
{
    [TextArea]
    public string effectDescription;

    protected Player player;

    public virtual bool CanBeUsed(Player player)
    {
        return true;
    }

    public virtual void ExecuteEffect()
    {
        
    }

    //订阅
    public virtual void Subscribe(Player player)
    {
        this.player = player;
    }

    //取消订阅
    public virtual void Unsubscribe()
    {
        
    }
}
