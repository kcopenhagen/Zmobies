using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject diePoolPanel;
    [SerializeField] private List<GameObject> thoughtPanels;
    [SerializeField] private Color blinkColor = Color.red;
    [SerializeField] TextMeshProUGUI diePoolButtonText;
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject zmobieThoughts;
    [SerializeField] TutorialManager tutorialManager;

    private readonly float blinkDuration = 0.1f;
    private readonly int blinkCount = 3;
    private Coroutine blinkRoutine;

    private DieSpawner dieSpawner;
    public int LevelTracker{ get; set; } = 0;
    
    private void Awake()
    {
        dieSpawner = GetComponent<DieSpawner>();

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void OpenOnly(GameObject panelToOpen)
    {
        if (!panelToOpen.activeInHierarchy && !CheckForOverlaps())
        {
            Transform current = panelToOpen.transform.parent;
            while (current != null)
            {
                if (!current.gameObject.activeSelf)
                    current.gameObject.SetActive(true);
                current = current.parent;
            }

            foreach (GameObject panel in thoughtPanels)
            {
                panel.SetActive(panel == panelToOpen);
            }
        }
    }

    public void OpenDiePoolPanelToggle(bool toggle)
    {
        if (diePoolPanel.activeSelf && toggle)
        {
            diePoolPanel.SetActive(false);
            diePoolButtonText.text = "Die pool";
        }
        else
        {
            diePoolPanel.SetActive(true);
            diePoolButtonText.text = "Box size controls";
        }
    }

    public void CloseMenuCheckOlaps()
    {
        if (!CheckForOverlaps())
        {
            menuPanel.SetActive(false);
            menuPanel.transform.parent.gameObject.SetActive(false);
        }
    }

    public bool CheckForOverlaps()
    {
        GameObject[] activeDice = GameObject.FindGameObjectsWithTag("Die");
        GameObject thoughtBox = GameObject.FindWithTag("ThoughtBox");

        if (activeDice.Length > 0 && thoughtBox != null)
        {
            List<GameObject> thoughtDice = new();

            foreach (GameObject die in activeDice)
            { if (die.transform.parent == thoughtBox.transform) thoughtDice.Add(die); }

            RectTransform rectTB = thoughtBox.GetComponent<RectTransform>();

            for (int i = 0; i < thoughtDice.Count; i++)
            {
                RectTransform rectA = thoughtDice[i].GetComponent<RectTransform>();

                if (!IsRectFullyInside(rectTB, rectA))
                {
                    TriggerBlink();
                    return true;
                }

                for (int j = i+1; j < thoughtDice.Count; j++)
                {
                    RectTransform rectB = thoughtDice[j].GetComponent<RectTransform>();
                    if (IsOverlapping(rectA, rectB))
                    {
                        TriggerBlink();
                        return true;
                    }
                }
            }
        }
        
        return false;
    }    

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector2 size = corners[2] - corners[0];
        return new Rect(corners[0], size);
    }

    private bool IsOverlapping(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);

        return rectA.Overlaps(rectB);
    }

    private bool IsRectFullyInside(RectTransform outer, RectTransform inner)
    {
        Rect outerRect = GetWorldRect(outer);
        Rect innerRect = GetWorldRect(inner);
        
        return outerRect.Contains(innerRect.min) && outerRect.Contains(innerRect.max);
    }

    private void TriggerBlink()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkBox());
    }
    private IEnumerator BlinkBox()
    {
        GameObject thoughtBox = GameObject.FindGameObjectWithTag("ThoughtBox");
        UnityEngine.UI.Image sr = thoughtBox.GetComponent<UnityEngine.UI.Image>();
        if (thoughtBox != null)
        {
            Color originalColor = sr.color;
            for (int i = 0; i < blinkCount; i++)
            {
                sr.color = blinkColor;
                yield return new WaitForSeconds(blinkDuration);
                sr.color = originalColor;
                yield return new WaitForSeconds(blinkDuration);
            }
            blinkRoutine = null;
        }
    }
    public void ClearSaveData()
    {
        tutorialManager.ResetProgress();

        //Destroy dice.
        List<GameObject> usedDice = dieSpawner.GetUsedDice();
        foreach (GameObject die in usedDice)
        {
            dieSpawner.TrashDie(die);
        }
        
        //Relock die buttons.
        DieSpawnCostManager[] allDieCosts = FindObjectsByType<DieSpawnCostManager>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (DieSpawnCostManager dice in allDieCosts)
            dice.RelockButton();
        
        //Relock doors.
        DoorCostManager[] allDoorCosts = FindObjectsByType<DoorCostManager>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (DoorCostManager door in allDoorCosts)
        {
            if (door.Source != "MainBase")
                door.RelockDoor();
        }
        
        //Reset currency.
        CurrencyManager.Instance.SetBodies(0);
        CurrencyManager.Instance.SetBrains(0);
        SaveManager.ClearSaveFile();
        welcomeScreen.SetActive(true);
        zmobieThoughts.SetActive(false);
    }
}
