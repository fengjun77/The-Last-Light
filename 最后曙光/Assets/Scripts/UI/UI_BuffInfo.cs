using UnityEngine;
using UnityEngine.UI;

public class UI_BuffInfo : MonoBehaviour
{
    [SerializeField] private GameObject uiBuffSlot;

    void OnEnable()
    {
        EventCenter.UpdateBuffIconEvent += UpdateBuffIcon;
    }

    void OnDisable()
    {
        EventCenter.UpdateBuffIconEvent -= UpdateBuffIcon;
    }

    private void UpdateBuffIcon(Sprite icon, float duration)
    {
        GameObject buff = Instantiate(uiBuffSlot, transform);
        if(buff != null)
            buff.GetComponent<UI_BuffSlot>().SetSlot(icon);

        Destroy(buff, duration);       
    }
}
