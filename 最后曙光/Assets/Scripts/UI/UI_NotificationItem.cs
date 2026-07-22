using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NotificationItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private LayoutElement layoutElement;

    [Header("动画设置")]
    [SerializeField] private float targetHeight = 60f; // 最终这个消息到达的高度
    [SerializeField] private float popUpSpeed = 10f; //展开高度的速度
    [SerializeField] private float lifetime = 3f; //基础显示时间
    //[SerializeField] private float fadeDuration = 1f;

    [Header("淡出设置")]
    [SerializeField] private float fadeStartPosY = 150f; //超过这个Y高度开始变淡
    [SerializeField] private float fadeDistance = 100f; //再往上飞多少距离彻底消失

    private float timer;
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        layoutElement.minHeight = 0;
        layoutElement.preferredHeight = 0;
        canvasGroup.alpha = 0;
    }

    public void SetUp(string message)
    {
        messageText.text = message;
        timer = 0;
        canvasGroup.alpha = 1;
    }

    private void Update()
    {   
        //如果高度没有到达目标高度，则用插值向目标高度平滑过度
        if(layoutElement.minHeight < targetHeight)
        {
            float newHeight = Mathf.Lerp(layoutElement.minHeight,targetHeight,Time.deltaTime * popUpSpeed);
            layoutElement.minHeight = newHeight;
            layoutElement.preferredHeight = newHeight;
        }

        timer += Time.deltaTime;

        if(timer > lifetime)
        {
            Destroy(gameObject);
            return;
        }

        //高度淡出逻辑
        //获取父物体局部坐标Y
        float currentY = rect.anchoredPosition.y;

        //随时间降低的透明度
        float timeAlpha = 1f;
        if(timer > lifetime - 1f)
            timeAlpha = lifetime - timer;

        float posAlpha = 1f;
        if(currentY > fadeStartPosY)
        {
            float progress = (currentY - fadeStartPosY) / fadeDistance;
            posAlpha = 1 - Mathf.Clamp01(progress);
        }

        canvasGroup.alpha = Mathf.Min(timeAlpha,posAlpha);

        if(canvasGroup.alpha <= 0.01f && timer > 1)
        {
            Destroy(gameObject);
        }
    }
}
