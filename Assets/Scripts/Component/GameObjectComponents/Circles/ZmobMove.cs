using System.Collections.Generic;
using UnityEngine;

public class ZmobMove : CircleMove
{
    public static List<CircleMove> All { get; } = new List<CircleMove>();


    protected override void OnDisable()
    {
        All.Remove(this);
        base.OnDisable();
    }
    protected override void OnEnable()
    {
        All.Add(this);
        base.OnEnable();
    }

}
