using System.Collections.Generic;
using UnityEngine;

public class UnlockDiceManager : MonoBehaviour
{
    public static UnlockDiceManager Instance;

    private HashSet<string> unlockedIDs = new();
    public HashSet<string> UnlockedDice => unlockedIDs;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool IsUnlocked(string id) => unlockedIDs.Contains(id);

    public bool TryUnlock(string id, int cost)
    {
        if (IsUnlocked(id)) return true;

        if (CurrencyManager.Instance.SpendBrains(cost))
        {
            unlockedIDs.Add(id);
            return true;
        }
        return false;
    }

    public void SetUnlockedDice(HashSet<string> unlockedDice)
    {
        if (unlockedDice != null)
            unlockedIDs = unlockedDice;

        DieSpawnCostManager[] allDiceButtons = FindObjectsByType<DieSpawnCostManager>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var dieButtons in allDiceButtons)
        {
            if (dieButtons == null)
            {
                Debug.LogWarning("Null die button found");
                continue;
            }

            if (string.IsNullOrEmpty(dieButtons.DieType))
            {
                Debug.LogWarning($"Panel {dieButtons.DieType} has null or empty Source.");
                continue;
            }
            if (unlockedIDs != null)
            {
                if (unlockedIDs.Contains(dieButtons.DieType))
                {
                    dieButtons.UnlockButton();
                }
            }
        }
    }

}
