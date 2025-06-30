using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    [SerializeField] private int brainsCount = 0;
    [SerializeField] private TextMeshProUGUI brainsText;
    [SerializeField] private int bodyCount = 0;
    [SerializeField] private TextMeshProUGUI bodyText;

    public int GetBrainsCount => brainsCount;
    public int GetBodyCount => bodyCount;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateBrainsUI();
        UpdateBodyUI();
    }

    public void AddBrains(int amount)
    {
        brainsCount += amount;
        UpdateBrainsUI();
    }

    public bool SpendBrains(int amount)
    {
        if (brainsCount >= amount)
        {
            brainsCount -= amount;
            UpdateBrainsUI();
            return true;
        }
        return false;
    }
    public void SetBrains(int amount)
    {
        brainsCount = amount;
        UpdateBrainsUI();
    }

    void UpdateBrainsUI()
    {
        brainsText.text = brainsCount.ToString();
    }


    public void AddBodies(int amount)
    {
        bodyCount += amount;
        UpdateBodyUI();
    }

    public bool SpendBodies(int amount)
    {
        if (bodyCount >= amount)
        {
            bodyCount -= amount;
            UpdateBodyUI();
            return true;
        }
        return false;
    }

    public void SetBodies(int amount)
    {
        bodyCount = amount;
        UpdateBodyUI();
    }

    void UpdateBodyUI()
    {
        bodyText.text = bodyCount.ToString();
    }


}
