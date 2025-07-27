using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private ObjectPool sickPool;
    [SerializeField] private ObjectPool healthyPool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate GameManager found and destroyed");
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    public ObjectPool GetHealthyPool => healthyPool;
    public ObjectPool GetSickPool => sickPool;

    private void OnDisable()
    {
        Debug.Log("Game manager disabled");
    }
}
