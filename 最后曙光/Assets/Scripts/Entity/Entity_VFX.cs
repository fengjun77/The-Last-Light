using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("受击效果")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = .2f;

    private Material originalMaterial;
    private Coroutine onDamageVfxCo;

    [Header("伤害效果")]
    [SerializeField] private Color hitVfxColor = Color.white;
    [SerializeField] private GameObject hitVFX;

    [SerializeField] private Color chillVfxColor = Color.blue;
    [SerializeField] private Color lightningColor = Color.yellow;
    private Color originalHitVfxColor;

    private Coroutine playStatusVfxCo;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
        originalHitVfxColor = hitVfxColor;
    }

    public void PlayOnStatusVfx(float duration, EffectType effectType)
    {
        if(playStatusVfxCo != null)
            StopCoroutine(playStatusVfxCo);

        switch(effectType)
        {
            case EffectType.Chill:
                StartCoroutine(PlayStatusVfxCo(duration, chillVfxColor));
                break;
            case EffectType.Lightning:
                StartCoroutine(PlayStatusVfxCo(duration, lightningColor));
                break;
        }
    }

    private IEnumerator PlayStatusVfxCo(float duration,Color effectColor)
    {
        float tickInterval = .25f;
        float timer = 0;

        Color lightColor = effectColor * 1.2f;
        Color darkColor = effectColor * .8f;

        bool toggle = false;
        while(timer < duration)
        {
            sr.color = toggle ? lightColor : darkColor;
            toggle = !toggle;

            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }

        sr.color = Color.white;
    }

    public void CreateOnHitVFX(Transform target)
    {
        GameObject vfx = Instantiate(hitVFX, target.position, Quaternion.identity);
        vfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor;
    }
    
    public void PlayeOnDamageVFX()
    {
        if(onDamageVfxCo != null)
            StopCoroutine(onDamageVfxCo);

        onDamageVfxCo = StartCoroutine(OnDamageVfxCo());
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }
}

public enum EffectType
{
    Chill,
    Fire,
    Lightning,
}
