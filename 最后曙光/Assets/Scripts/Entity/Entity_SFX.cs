using UnityEngine;

public class Entity_SFX : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private string attack;
    [SerializeField] private string attackMiss;
    [SerializeField] private string move;
    [SerializeField] private string jump;
    [SerializeField] private string dash;
    [SerializeField] private string heal;
    [SerializeField] private string lightning;
    [SerializeField] private string slowDown;

    [SerializeField] private float soundDistance = 15f;

    void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayAttack()
    {
        AudioManager.instance.PlaySFX(attack, audioSource);
    }

    public void PlayAttackMiss()
    {
        AudioManager.instance.PlaySFX(attackMiss, audioSource);
    }

    public void PlayJump()
    {
        AudioManager.instance.PlaySFX(jump, audioSource);
    }

    public void PlayDash()
    {
        AudioManager.instance.PlaySFX(dash, audioSource);
    }

    public void PlayHeal()
    {
        AudioManager.instance.PlaySFX(heal, audioSource);
    }

    public void PlayLightning()
    {
        AudioManager.instance.PlaySFX(lightning, audioSource);
    }

    public void PlaySlowEffect()
    {
        AudioManager.instance.PlaySFX(slowDown, audioSource);
    }

    public void PlayMoveSource()
    {
        AudioManager.instance.PlaySFX(move, audioSource, 15, true);
    }

    public void StopMoveSource()
    {
        audioSource.Stop();
    }
}
