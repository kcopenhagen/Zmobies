using UnityEngine;

public class GameSaver : MonoBehaviour
{
    [SerializeField] private UnlockDoorManager unlockDoorManager;
    [SerializeField] private UnlockDiceManager unlockDiceManager;
    [SerializeField] private RectTransform mainBaseTB;
    [SerializeField] private RectTransform door1TB;
    [SerializeField] private RectTransform door2TB;
    [SerializeField] private RectTransform door3TB;
    [SerializeField] private RectTransform sicknessTB;

    private void OnApplicationQuit()
    {
        float mainBaseTBw = mainBaseTB.rect.size.x;
        float mainBaseTBh = mainBaseTB.rect.size.y;
        float door1TBw = door1TB.rect.size.x;
        float door1TBh = door1TB.rect.size.y;
        float door2TBw = door2TB.rect.size.x;
        float door2TBh = door2TB.rect.size.y;
        float door3TBw = door3TB.rect.size.x;
        float door3TBh = door3TB.rect.size.y;
        float sicknessTBw = sicknessTB.rect.size.x;
        float sicknessTBh = sicknessTB.rect.size.y;

        SaveManager.Save(UIManager.Instance.GetComponent<DieSpawner>().GetUsedSettings(),
            CurrencyManager.Instance.GetBrainsCount, CurrencyManager.Instance.GetBodyCount,
            unlockDoorManager.UnlockedDoors, unlockDiceManager.UnlockedDice, mainBaseTBw,
            mainBaseTBh, door1TBw, door1TBh, door2TBw, door2TBh, door3TBw, door3TBh, 
            sicknessTBw, sicknessTBh);
    }
}
