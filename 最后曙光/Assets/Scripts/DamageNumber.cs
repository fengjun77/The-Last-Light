using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    private TextMeshProUGUI text;
    private Color originColor;

    public DamageNumber damagePrefab;

    [SerializeField] private float duration = .4f;
    [SerializeField] private float riseSpeed = 5f;
    [SerializeField] private int normalFontSize = 44;
    [SerializeField] private int critFontSize = 55;
    [SerializeField] private Color critColor = Color.red;
    
    private float timer;

    private bool isRunning;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originColor = text.color;
    }

    public void SetValue(float damage, bool isCrit)
    {
        timer = 00;
        isRunning = true;
        if(isCrit)
        {
            text.fontSize = critFontSize;
            text.color = critColor;
        }
        else
        {
            text.fontSize = normalFontSize;
            text.color = originColor;
        }

        text.text = damage.ToString();
    }

    public void Reset()
    {
        timer = 0;
        isRunning = false;
        Color textColor = text.color;
        textColor.a = 1;
        text.color = textColor;
    }

    void Update()
    {
        if(!isRunning) return;
        timer += Time.deltaTime;

        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);

        float alpha = 1 - timer / duration;
        Color c = text.color;
        c.a = alpha;
        text.color = c;

        if(timer >= duration)
        {
            Reset();
            PoolManager.Instance.ReturnItem<DamageNumber>(damagePrefab, this);
        }
    }
}
