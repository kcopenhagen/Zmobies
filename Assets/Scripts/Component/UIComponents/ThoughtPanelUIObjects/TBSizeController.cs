using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class TBSizeController : MonoBehaviour
{
    [SerializeField] Transform thoughtBox;
    [SerializeField] TextMeshProUGUI processingTimeText;
    [SerializeField] TextMeshProUGUI spawningTimeText;
    [SerializeField] TextMeshProUGUI procEfficiency;
    [SerializeField] TextMeshProUGUI spawnEfficiency;

    private float wMin = 100f;
    private float wMax = 1200f;
    private float hMin = 100f;
    private float hMax = 1200f;

    private readonly float boxWidth2DecisionTimeFactor = 1f / 200f;
    private readonly float boxHeight2SpawnTimeFactor = 1f / 100f;

    private RectTransform TBRect;
    private void Awake()
    {
        TBRect = thoughtBox.GetComponent<RectTransform>();

    }
    public void IncreaseProcessingTime()
    {
        float w = TBRect.rect.width;
        float h = TBRect.rect.height;

        if (w + 50f <= wMax)
        {
            w += 50f;
            TBRect.sizeDelta = new Vector2(w, h);
            UpdateTimes();
        }
    }
    public void DecreaseProcessingTime()
    {
        float w = TBRect.rect.width;
        float h = TBRect.rect.height;

        if (w - 50f >= wMin)
        {
            w -= 50;
            TBRect.sizeDelta = new Vector2(w, h);
            UpdateTimes();
        }
    }
    public void IncreaseSpawningTime()
    {
        float w = TBRect.rect.width;
        float h = TBRect.rect.height;

        if (h + 50f <= hMax)
        {
            h += 50f;
            TBRect.sizeDelta = new Vector2(w, h);
            UpdateTimes();
        }
    }
    public void DecreaseSpawningTime()
    {
        float w = TBRect.rect.width;
        float h = TBRect.rect.height;
        
        if (h - 50f >= hMin)
        {
            h -= 50f;
            TBRect.sizeDelta = new Vector2(w, h);
            UpdateTimes();
        }
    }
    public void UpdateTimes()
    {
        TBRect = thoughtBox.GetComponent<RectTransform>();
        float w = TBRect.rect.width;
        float h = TBRect.rect.height;
        float procEfficiencyFactor;
        float spawnEfficiencyFactor;

        string procCleaned = Regex.Replace(procEfficiency.text, @"[^0-9\.\-]", "");
        if (float.TryParse(procCleaned, out procEfficiencyFactor))
            procEfficiencyFactor = 1f + procEfficiencyFactor / 100f;
        else
            procEfficiencyFactor = 1f;

        string spawnCleaned = Regex.Replace(spawnEfficiency.text, @"[^0-9\.\-]", "");
        if (float.TryParse(spawnCleaned, out spawnEfficiencyFactor))
            spawnEfficiencyFactor = 1f + spawnEfficiencyFactor / 100f;
        else
            spawnEfficiencyFactor = 1f;

        float procTime = boxWidth2DecisionTimeFactor * w / procEfficiencyFactor;
        float spawnTime = boxHeight2SpawnTimeFactor * h / spawnEfficiencyFactor;
        processingTimeText.text = procTime.ToString("F1");
        spawningTimeText.text = spawnTime.ToString("F1");
    }
}
