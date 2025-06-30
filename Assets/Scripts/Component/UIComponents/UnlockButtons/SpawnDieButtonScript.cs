using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpawnDieButtonScript : MonoBehaviour
{
    [SerializeField] private List<int> sidesPool;
    [SerializeField] private Vector2 target = Vector2.zero;

    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    
    [SerializeField] private string dieType;

    [SerializeField] private GameObject diePoolPanel;
    [SerializeField] private GameObject dieSpawner;
    [SerializeField] private GameObject infoPanel;

    private void Awake()
    {
        GetComponent<DieSpawnCostManager>().OnUnlockedClick += SpawnDie;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Image im = GetComponent<Image>();

        if (im != null)
            im.color = ColorManager.Instance.SelectDieColor(dieType);
    }

    private void SpawnDie()
    {
        DieSettings settings = new()
        {
            size = Random.Range(minSize, maxSize),
            type = dieType,
            target = target,
            source = "DiePool"
        };

        for (int i = 0; i < settings.sideValues.Length; i++)
        {
            int index = Random.Range(0, sidesPool.Count);
            settings.sideValues[i] = sidesPool[index];
        }

        Rect diePoolrect = diePoolPanel.GetComponent<RectTransform>().rect;

        // Set it's position to the top half of the pool to avoid buttons, and avoid far right where trash is.
        settings.position = new Vector2(
            Random.Range(diePoolrect.xMin,diePoolrect.xMax-300f),
            Random.Range((diePoolrect.yMin+diePoolrect.yMax)/2f,diePoolrect.yMax)); 

        dieSpawner.GetComponent<DieSpawner>().SpawnSpecialDie(settings, diePoolPanel.GetComponent<Transform>());
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }
    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
}
