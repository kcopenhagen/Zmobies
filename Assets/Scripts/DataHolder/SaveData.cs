using System.Collections.Generic;
using System.Numerics;

[System.Serializable]
public class SaveData
{
    public List<DieSettings> diePool;
    public int brains;
    public int bodies;
    public List<string> unlockedDoors;
    public List<string> unlockedDice;

    public float mainBaseTBw;
    public float mainBaseTBh;
    public float door1TBw;
    public float door1TBh;
    public float door2TBw;
    public float door2TBh;
    public float door3TBw;
    public float door3TBh;
    public float sicknessTBw;
    public float sicknessTBh;

}
