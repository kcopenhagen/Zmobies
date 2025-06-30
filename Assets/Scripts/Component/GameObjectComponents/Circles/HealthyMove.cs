using System.Collections.Generic;
using UnityEngine;

public class HealthyMove : CircleMove
{
    public static List<CircleMove> All { get; } = new List<CircleMove>();
    private bool running;
    private float runSpeed = 5f;

    public void RunAway(bool run)
    {
        running = run;
    }
    protected override void RollDiceInternal()
    {
        if (!running)
            base.RollDiceInternal();
        else
        {
            moveDirection = rb.position.normalized;
            speed = runSpeed;
        }
    }

    protected override void OnDisable()
    {
        running = false;
        All.Remove(this);
        base.OnDisable();
    }
    protected override void OnEnable()
    {
        running = false;
        All.Add(this);
        base.OnEnable();
    }
}
