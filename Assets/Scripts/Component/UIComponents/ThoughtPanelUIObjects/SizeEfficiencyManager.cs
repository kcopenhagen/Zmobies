using System.ComponentModel;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class SizeEfficiencyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI procEffCost;
    [SerializeField] private TextMeshProUGUI spawnEffCost;
    [SerializeField] private TextMeshProUGUI procEffText;
    [SerializeField] private TextMeshProUGUI spawnEffText;
    
    private TBSizeController tBSizeController;
    private CurrencyManager currencyManager;
    void Awake()
    {
        currencyManager = CurrencyManager.Instance;
        tBSizeController = GetComponent<TBSizeController>();
    }

    public void IncreaseProcEff()
    {
        int procCost;
        string procCleaned = Regex.Replace(procEffCost.text, @"[^0-9\.\-]", "");
        if (int.TryParse(procCleaned, out procCost))
        {
            if (currencyManager.SpendBrains(procCost))
            {
                int newCost = procCost + 5;
                procEffText.text = "+" + (procCost) + "%";
                procEffCost.text = newCost.ToString();
                tBSizeController.UpdateTimes();
            }
        }
    }
    public void IncreaseSpawnEff()
    {
        int spawnCost;
        string spawnCleaned = Regex.Replace(spawnEffCost.text, @"[^0-9\.\-]", "");
        if (int.TryParse(spawnCleaned, out spawnCost))
        {
            if (currencyManager.SpendBrains(spawnCost))
            {
                int newCost = spawnCost + 5;
                spawnEffText.text = "+" + (spawnCost) + "%";
                spawnEffCost.text = newCost.ToString();
                tBSizeController.UpdateTimes();
            }
        }
    }
}
