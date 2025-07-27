
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DoorCostManager : MonoBehaviour
{
    [SerializeField] private UIButtonClickManager button;
    [SerializeField] private string source;
    public string Source => source;
    [SerializeField] private int unlockCost = 0;
    [SerializeField] GameObject doorSpawner;
    [SerializeField] TutorialManager tutorialManager;

    [SerializeField] private GameObject lockIcon;
    public System.Action OnUnlockedClick;

    private bool isUnlocked = false;

    private void Awake()
    {
        if (!button) button = GetComponent<UIButtonClickManager>();
        button.OnClick += HandleClick;
    }

    private void Start()
    {
        if (source == "MainBase" || source == "Healthy")
        {
            if (UnlockDoorManager.Instance.TryUnlock(source, 0))
            {
                isUnlocked = true;
                lockIcon.SetActive(false);
            }
        }
    }

    private void HandleClick()
    {
        if (!isUnlocked)
        {
            if (UnlockDoorManager.Instance == null)
                Debug.Log("no unlock door manager instance found");
            if (UnlockDoorManager.Instance.TryUnlock(source, unlockCost))
            {
                isUnlocked = true;
                if (lockIcon != null)
                {
                    lockIcon.SetActive(false);
                }
                if (doorSpawner != null)
                    doorSpawner.SetActive(true);
            }
            else
            {
                Debug.Log("Not enough brains to unlock.");
            }
            return;
        }
        tutorialManager.Tutorial2Done();
        OnUnlockedClick?.Invoke();

    }
    public void OpenDoor()
    {
        isUnlocked = true;
        if (lockIcon != null)
            lockIcon.SetActive(false);
        else
            Debug.LogWarning($"{gameObject.name} missing lockIcon reference.");
        if (doorSpawner != null)
            doorSpawner.SetActive(true);
        else
            Debug.LogWarning($"{gameObject.name} missing doorSpawner reference.");
    }
    public void RelockDoor()
    {
        isUnlocked = false;
        if (lockIcon != null)
        {
            lockIcon.SetActive(true);
        }
        if (doorSpawner != null && source != "Sickness" && source != "Healthy")
        {
            doorSpawner.SetActive(false);
        }
    }


}
