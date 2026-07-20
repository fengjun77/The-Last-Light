using UnityEngine;

public class Player_VFX : Entity_VFX
{

    public void CreatePlayerVfx(GameObject vfx, Vector3 target)
    {
        Instantiate(vfx, target, Quaternion.identity);
    }
}
