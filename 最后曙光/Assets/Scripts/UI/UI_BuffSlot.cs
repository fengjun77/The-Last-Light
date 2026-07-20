using UnityEngine;
using UnityEngine.UI;

public class UI_BuffSlot : MonoBehaviour
{
    public Image buffIcon;

    public void SetSlot(Sprite icon)
    {
        buffIcon.sprite = icon;
    }
}
