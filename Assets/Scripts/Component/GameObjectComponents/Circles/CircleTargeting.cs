using System.Collections.Generic;
using UnityEngine;

public class CircleTargeting : MonoBehaviour
{
    private CircleMove move;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        move = GetComponent<CircleMove>();
    }

    public void SetFollowingTarget()
    {
        CircleMove target = null;
        CircleMove closestZmob = null;
        float minDist = 99999f;

        foreach (var mover in ZmobMove.All)
        {
            if (mover == move) continue;

            var moverRb = mover.GetComponent<Rigidbody2D>();
            float dist = (rb.position - moverRb.position).magnitude;

            if (mover.PlayerControl != null && mover.PlayerControl.IsControlled)
            {
                target = mover;
                break;
            }

            if (dist < minDist)
            {
                closestZmob = mover;
                minDist = dist;
            }
        }

        foreach (var die in move.MyDice)
        {
            if (die.type == "Following" && target != null)
                die.target = target.transform.position;

            if (die.type == "Evading" && closestZmob != null)
                die.target = closestZmob.transform.position;
        }
    }
}
