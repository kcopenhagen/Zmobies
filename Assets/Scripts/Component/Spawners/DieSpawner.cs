using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using NUnit.Framework;

public class DieSpawner : MonoBehaviour
{

    [SerializeField] private GameObject diePrefab;
    [SerializeField] int poolSize = 100;
    [SerializeField] DiceInspectorZone inspectorZone;
    [SerializeField] private Transform spawnParent;
    [SerializeField] private GameObject MainBaseTB;
    [SerializeField] private GameObject Door1TB;
    [SerializeField] private GameObject Door2TB;
    [SerializeField] private GameObject Door3TB;
    [SerializeField] private GameObject SicknessTB;
    [SerializeField] private GameObject HealthyTB;

    private List<GameObject> diePool = new();
    private List<GameObject> availableDice = new();
    private DieInfoPanel infoPanel;



    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject die = Instantiate(diePrefab, spawnParent);
            die.SetActive(false);
            diePool.Add(die);
            availableDice.Add(die);
        }
    }

    public void SpawnSpecialDie(DieSettings settings, Transform parentPanel)
    {
        if (availableDice.Count == 0)
        {
            Debug.LogWarning("No more dice");
            return;
        }

        GameObject die = availableDice[0];
        availableDice.RemoveAt(0);
        if (die != null)
        {
            die.SetActive(true);
            die.transform.SetParent(parentPanel);

            if (die.TryGetComponent<RectTransform>(out var rect))
            {
                rect.anchoredPosition = settings.position;
                rect.sizeDelta = new Vector2(settings.size, settings.size);
            }
            if (die.TryGetComponent<CanvasGroup>(out var group))
                group.blocksRaycasts = true;

            settings.guid = Guid.NewGuid().ToString();

            Image im = die.GetComponent<Image>();
            if (im != null)
                die.GetComponent<Image>().color = ColorManager.Instance.SelectDieColor(settings.type);
            
            die.GetComponent<DieMove>().SetInspectorZone(inspectorZone);
            die.GetComponent<DieMove>().SetSettings(settings);
            die.GetComponent<DieMove>().UpdateDieFace();
            infoPanel = die.GetComponentInChildren<DieInfoPanel>(true);
            if (infoPanel != null)
                infoPanel.UpdateDieInfoPanel(settings);
            else
                Debug.LogWarning("No die info panel found");
        }
    }

    public void TrashDie(GameObject die)
    {
        die.transform.SetParent(spawnParent);
        die.SetActive(false);
        die.GetComponent<DieMove>().OnTrash();
        if (!availableDice.Contains(die))
        {
            availableDice.Add(die);
        }
    }


    public List<GameObject> GetAvailableDice()
    {
        return availableDice;
    }

    public List<GameObject> GetUsedDice()
    {
        List<GameObject> usedDice = new();
        foreach (GameObject die in diePool)
        {
            if (!availableDice.Contains(die))
                usedDice.Add(die);
        }
        return usedDice;
    }

    public List<DieSettings> GetUsedSettings()
    {
        List<DieSettings> usedSettings = new();
        foreach (GameObject die in diePool)
        {
            if (!availableDice.Contains(die))
                usedSettings.Add(die.GetComponent<DieMove>().GetSettings);
        }
        return usedSettings;
    }
    public List<DieSettings> GetDiceOfType(string dieType)
    {
        List<DieSettings> typedDice = new();
        foreach (GameObject die in GetUsedDice())
        {
            DieSettings settings = die.GetComponent<DieMove>().GetSettings;
            if (settings != null)
            {
                if (settings.type == dieType && settings.source != "Healthy") typedDice.Add(settings);
            }
            else
                Debug.Log("No die settings found in GetDiceOfType");
        }
        return typedDice;
    }
    
    public List<DieSettings> GetDiceOfSource(string dieSource)
    {
        List<DieSettings> sourcedDice = new();
        foreach (GameObject die in GetUsedDice())
        {
            DieSettings settings = die.GetComponent<DieMove>().GetSettings;
            if (settings != null)
            {
                if (settings.source == dieSource) sourcedDice.Add(settings);
            }
            else
                Debug.Log("No die settings found in GetDiceOfSource");
        }
        return sourcedDice;
    }

    public void LoadDice(List<DieSettings> savedDice)
    {
        foreach (DieSettings settings in savedDice)
        {
            GameObject parentPanel = null;
            switch (settings.source)
            {
                case "MainBase":
                    parentPanel = MainBaseTB; break;
                case "Door1":
                    parentPanel = Door1TB; break;
                case "Door2":
                    parentPanel = Door2TB; break;
                case "Door3":
                    parentPanel = Door3TB; break;
                case "Sickness":
                    parentPanel = SicknessTB; break;
                case "Healthy":
                    parentPanel = HealthyTB; break;
            }
            SpawnSpecialDie(settings, parentPanel.transform);
        }
    }
}
