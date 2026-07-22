using System;
using UnityEngine;

public class Object_CheckPoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpointId;
    [SerializeField] private Transform respawnPoint;
    public bool isActive { get; private set; }
    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if(string.IsNullOrEmpty(checkpointId))
            checkpointId = System.Guid.NewGuid().ToString();
#endif 
    }

    public string GetCheckpointID() => checkpointId;

    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;

    /// <summary>
    /// 激活/取消保存点
    /// </summary>
    /// <param name="activate"></param>
    public void ActivateCheckpoint(bool activate)
    {
        isActive = activate;
        anim.SetBool("isActive", activate);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(isActive == false) EventCenter.OnShowNotificationEvent("成功激活此检查点");
        ActivateCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active = data.unlockedCheckpoints.TryGetValue(checkpointId, out active);
        ActivateCheckpoint(active);
    }

    public void SaveData(ref GameData data)
    {
        if(!isActive) return;

        if(data.unlockedCheckpoints.ContainsKey(checkpointId) == false)
        {
            data.unlockedCheckpoints.Add(checkpointId, true);
        }
    }
}
