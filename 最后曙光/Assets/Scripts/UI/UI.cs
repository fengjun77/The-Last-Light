using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput { get; private set; }
    private PlayerInputSet input;

    public UI_SkillToolTip skillToolTip { get; private set; }
    public UI_ItemToolTip itemToolTip { get; private set; }

    //玩家背包以及信息面板
    public UI_Inventory inventoryUI { get; private set; }
    public UI_Storage storageUI { get; private set; }
    public UI_Craft craftUI { get; private set; }
    public UI_Merchant merchantUI { get; private set; }
    public UI_InGame inGameUI { get; private set; }
    public UI_Options optionsUI { get; private set; }
    public UI_DeathScreen deathScreenUI { get; private set; }
    public UI_FadeScreen fadeScreenUI { get; private set; } 

    [Header("消息提示")]
    [SerializeField] private GameObject notificationItem;
    [SerializeField] private Transform notificaitonContainer;
    [SerializeField] private float msgInterval = .4f; //消息显示间隔时间

    private Queue<string> messageQueue = new Queue<string>(); //消息队列
    private bool isDisplaying = false; //是否正在处理队列的循环中

    private bool inventoryEnabled;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        inventoryUI = GetComponentInChildren<UI_Inventory>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);
        deathScreenUI = GetComponentInChildren<UI_DeathScreen>(true);
        fadeScreenUI = GetComponentInChildren<UI_FadeScreen>(true);

        inventoryEnabled = inventoryUI.gameObject.activeSelf;
    }

    void OnEnable()
    {
        EventCenter.ShowNotificationEvent += ShowNotifiation;
    }

    void OnDisable()
    {
        EventCenter.ShowNotificationEvent -= ShowNotifiation;
    }

    public void SetupControlsUI(PlayerInputSet input)
    {
        this.input = input;

        input.UI.InventoryUI.performed += ctx => ToggleInventoryUI();
        input.UI.OptionsUI.performed += ctx => 
        {
            foreach(var element in uiElements)
            {  
                //如果有面板处于激活，则关闭
                if(element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }

            Time.timeScale = 0;
            //打开设置面板
            OpenOptionUI();
        };

        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;
    
    }

    public void OpenDeathScreenUI()
    {
        SwitchTo(deathScreenUI.gameObject);

        HideAllToolTips();
        input.Disable();
    }

    /// <summary>
    /// 打开设置面板
    /// </summary>
    public void OpenOptionUI()
    {
        SwitchTo(optionsUI.gameObject);

        HideAllToolTips();
        StopPlayerControls(true);
    }

    /// <summary>
    /// 关闭所有UI面板（除游戏内UI）
    /// </summary>
    public void SwitchToInGameUI()
    {
        HideAllToolTips();
        StopPlayerControls(false);
        
        SwitchTo(inGameUI.gameObject);

        inventoryEnabled = false;
    }

    private void SwitchTo(GameObject objectToSwitchOn)
    {
        foreach(var element in uiElements)
        {
            element.gameObject.SetActive(false);
        }

        objectToSwitchOn.SetActive(true);
    }

    private void StopPlayerControlsIfNeed()
    {
        foreach(var element in uiElements)
        {
            if(element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }

        StopPlayerControls(false);
    }

    private void StopPlayerControls(bool stopControls)
    {
        if(stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }

    public void ToggleInventoryUI()
    {
        inventoryUI.transform.SetAsLastSibling();
        SetTooltipsAboveOtherElements();
        fadeScreenUI.transform.SetAsLastSibling();

        inventoryEnabled = !inventoryEnabled;
        inventoryUI.gameObject.SetActive(inventoryEnabled);
        //if(inventoryEnabled) EventCenter.OnInventoryChangeEvent();
        HideAllToolTips();

        StopPlayerControlsIfNeed();
    }

    public void OpenCraftUI(bool openCraftUI)
    {
        craftUI.gameObject.SetActive(openCraftUI);
        StopPlayerControls(openCraftUI);

        if(!openCraftUI)
        {
            storageUI.gameObject.SetActive(false);
            HideAllToolTips();
        }
    }

    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControls(openMerchantUI);

        if(!openMerchantUI)
            HideAllToolTips();
    }

    
    public void HideAllToolTips()
    {
        skillToolTip.ShowToolTip(false, null);
        itemToolTip.ShowToolTip(false,null);
    }

    private void SetTooltipsAboveOtherElements()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
    }

    public void ShowNotifiation(string message)
    {
        messageQueue.Enqueue(message);

        if(!isDisplaying)
            StartCoroutine(DisplayQueueCoroutine());
    }

    private IEnumerator DisplayQueueCoroutine()
    {
        isDisplaying = true;

        while(messageQueue.Count > 0)
        {
            string msg = messageQueue.Dequeue();

            CreateNotification(msg);

            yield return new WaitForSeconds(msgInterval);
        }

        isDisplaying = false;
    }

    private void CreateNotification(string msg)
    {
        GameObject newMsg = Instantiate(notificationItem, notificaitonContainer);

        newMsg.transform.SetAsLastSibling();

        UI_NotificationItem itemScript = newMsg.GetComponent<UI_NotificationItem>();

        if(itemScript != null)
            itemScript.SetUp(msg);
    }
}
