
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DieSpawnCostManager : MonoBehaviour
{
    [SerializeField] private UIButtonClickManager button;
    [SerializeField] private string dieType;
    public string DieType => dieType;
    [SerializeField] private int unlockCost = 0;
    [SerializeField] private int baseClickCost = 0;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject dieSpawnCostTextBox;

    public System.Action OnUnlockedClick;
    private int clickCost;
    private bool isUnlocked = false;
    private TextMeshProUGUI dieSpawnCostText;

    private void Awake()
    {
        if (!button) button = GetComponent<UIButtonClickManager>();
        button.OnClick += HandleClick;
        clickCost = baseClickCost;
        dieSpawnCostText = dieSpawnCostTextBox.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        UpdateClickCost();
    }

    private void HandleClick()
    {
        if (!isUnlocked)
        {
            if (UnlockDiceManager.Instance.TryUnlock(dieType, unlockCost))
            {
                isUnlocked = true;
                if (lockIcon != null)
                {
                    dieSpawnCostTextBox.SetActive(true);
                    lockIcon.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Not enough brains to unlock.");
            }
            return;
        }


        if (CurrencyManager.Instance.SpendBrains(clickCost))
        {
            OnUnlockedClick?.Invoke();
        }
        else
        {
            Debug.Log("Not enough brains to spawn die.");
        }
    }

    public void UpdateClickCost()
    {
        DieSpawner dieSpawner = null;
        
        dieSpawner = UIManager.Instance.GetComponent<DieSpawner>();
        if (dieSpawner == null)
        {
            Debug.LogWarning("DieSpawner component not found on UIManager");
        }
        List<DieSettings> usedDice = dieSpawner.GetDiceOfType(dieType);
        clickCost = baseClickCost * (1 + usedDice.Count);
        dieSpawnCostText.text = clickCost.ToString();
    }
    
    public void RelockButton()
    {
        if (lockIcon != null)
        {
            dieSpawnCostTextBox.SetActive(false);
            lockIcon.SetActive(true);
        }
        isUnlocked = false;
    }

    public void UnlockButton()
    {
        isUnlocked = true;
        if (lockIcon != null)
            lockIcon.SetActive(false);
        else
            Debug.LogWarning($"{gameObject.name} missing lockIcon reference.");

    }
}
