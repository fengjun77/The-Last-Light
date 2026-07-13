using System.Runtime.InteropServices;
using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestory = true;
    [SerializeField] private float destoryDelay = .5f;
    [Space]
    [SerializeField] private bool randomOffset = true;

    [Header("随机位置")]
    [SerializeField] private float xMinOffset = -.3f;
    [SerializeField] private float xMaxOffset = .3f;
    [Space]
    [SerializeField] private float yMinOffset = -.3f;
    [SerializeField] private float yMaxOffset = .3f;

    void Start()
    {
        ApplyRandomOffset();

        if(autoDestory)
            Destroy(gameObject, destoryDelay);
    }

    private void ApplyRandomOffset()
    {
        if(!randomOffset) return;

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position = transform.position + new Vector3(xOffset,yOffset);
    }
}
