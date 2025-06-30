
using UnityEngine;

[System.Serializable]
public class DieSettings
{
    public float size;
    public float[] sideValues = new float[6];
    public string source;
    public string type;  //Speed, Homing, Wandering, Following 
    public string guid;
    public Vector2 position;
    public Vector2 target = Vector2.zero;

    public DieSettings Clone()
    {
        return new DieSettings
        {
            size = this.size,
            sideValues = (float[])this.sideValues.Clone(),
            source = this.source,
            type = this.type,
            guid = this.guid,
            position = this.position,
            target = this.target
        };
    }
}

