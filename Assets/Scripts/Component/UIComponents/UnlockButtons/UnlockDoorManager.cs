using System.Collections.Generic;
using UnityEngine;

public class UnlockDoorManager : MonoBehaviour
{
    public static UnlockDoorManager Instance;

    private HashSet<string> unlockedIDs = new();
    public HashSet<string> UnlockedDoors => unlockedIDs;

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
    public void SetUnlockedDoors(HashSet<string> unlockedDoors)
    {
        if (unlockedDoors != null)
            unlockedIDs = unlockedDoors;

        DoorCostManager[] allThoughtPanels = FindObjectsByType<DoorCostManager>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        
        foreach (var panel in allThoughtPanels)
        {
            if (panel == null)
            {
                Debug.LogWarning("Null panel found");
                continue;
            }

            if (string.IsNullOrEmpty(panel.Source))
            {
                Debug.LogWarning($"Panel {panel.name} has null or empty Source.");
                continue;
            }
            if (unlockedIDs != null)
            {
                if (unlockedIDs.Contains(panel.Source))
                {
                    panel.OpenDoor();
                }
            }
        }
    }
}
