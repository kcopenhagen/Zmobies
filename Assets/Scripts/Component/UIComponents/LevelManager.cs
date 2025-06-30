using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<GameObject> levels = new();
    [SerializeField] Transform healthyPanel;
    [SerializeField] HealthySpawner healthySpawner;
    [SerializeField] TextMeshProUGUI healthyProcEff;
    [SerializeField] TextMeshProUGUI healthySpawnEff;
    [SerializeField] TextMeshProUGUI healthyButtonText;
    [SerializeField] TBSizeController healthyTBSizeController;
    private DieSpawner dieSpawner;
    

    private void Start()
    {
        dieSpawner = UIManager.Instance.GetComponent<DieSpawner>();
    }
    private void ClearHealthy()
    {
        List<GameObject> healthyDice = dieSpawner.GetUsedDice();
        foreach (GameObject die in healthyDice)
        {
            DieSettings settings = die.GetComponent<DieMove>().GetSettings;
            if (settings != null && settings.source == "Healthy")
                dieSpawner.TrashDie(die);
        }
    }

    public void SetLevel(int level)
    {
        
        GameObject newLevel = levels[level];
        if (newLevel == null)
            return;

        UIManager.Instance.LevelTracker = level;

        ClearHealthy();
        healthyButtonText.text = "Level " + level.ToString();

        foreach (Transform child in newLevel.GetComponentsInChildren<Transform>(true))
        {
            
            if (child.CompareTag("ProcEff"))
            {
                healthyProcEff.text = child.GetComponent<TextMeshProUGUI>().text.ToString();
            }
            if (child.CompareTag("SpawnEff"))
            {
                healthySpawnEff.text = child.GetComponent<TextMeshProUGUI>().text.ToString();
            }
        }
        foreach (Transform child in newLevel.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag("LevelThoughtBox"))
            {
                float w = child.GetComponent<RectTransform>().rect.width;
                float h = child.GetComponent<RectTransform>().rect.height;

                RectTransform rt = healthyPanel.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(w, h);

                healthyTBSizeController.UpdateTimes();
                healthySpawner.UpdateNumbers();

                foreach (Transform die in child.transform)
                {
                    if (die.CompareTag("Die"))
                    {
                        DieSettings settings = die.GetComponent<DieMove>().GetSettings;
                        settings.position = die.GetComponent<RectTransform>().anchoredPosition;
                        settings.source = "Healthy";

                        dieSpawner.SpawnSpecialDie(settings, healthyPanel);
                    }
                }
            }
        }
    }
}
