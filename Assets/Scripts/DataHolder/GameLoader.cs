using UnityEngine;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private DieSpawner dieSpawner;
    [SerializeField] private UnlockDoorManager unlockDoorManager;
    [SerializeField] private UnlockDiceManager unlockDiceManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private RectTransform mainBaseTB;
    [SerializeField] private RectTransform door1TB;
    [SerializeField] private RectTransform door2TB;
    [SerializeField] private RectTransform door3TB;
    [SerializeField] private RectTransform sicknessTB;

    private void Start()
    {
        SaveData data = SaveManager.Load();

        if (data!= null)
        {
            dieSpawner.LoadDice(data.diePool);
            CurrencyManager.Instance.SetBrains(data.brains);
            CurrencyManager.Instance.SetBodies(data.bodies);
            HashSet<string> unlockedDoors = new(data.unlockedDoors);
            unlockDoorManager.SetUnlockedDoors(unlockedDoors);
            HashSet<string> unlockedDice = new(data.unlockedDice);
            unlockDiceManager.SetUnlockedDice(unlockedDice);

            mainBaseTB.sizeDelta = new Vector2(data.mainBaseTBw, data.mainBaseTBh);
            door1TB.sizeDelta = new Vector2(data.door1TBw, data.door1TBh);
            door2TB.sizeDelta = new Vector2(data.door2TBw, data.door2TBh);
            door3TB.sizeDelta = new Vector2(data.door3TBw, data.door3TBh);
            sicknessTB.sizeDelta = new Vector2(data.sicknessTBw, data.sicknessTBh);
        }
        levelManager.SetLevel(0);
    }
}
