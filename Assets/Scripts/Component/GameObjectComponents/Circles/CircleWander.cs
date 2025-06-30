using System.Collections;
using UnityEngine;

public class CircleWander : MonoBehaviour
{
    [SerializeField] private float directionChangeInterval = 1f;

    private Coroutine wanderCoroutine;
    private CircleMove move;
    private CirclePlayerControl playerControl;
    private CircleTargeting targeting;

    private void Awake()
    {
        move = GetComponent<CircleMove>();
        playerControl = GetComponent<CirclePlayerControl>();
        if (playerControl == null)
            Debug.Log("No player control in awake");
        targeting = GetComponent<CircleTargeting>();
    }

    public void StartWandering()
    {
        if (wanderCoroutine == null)
            wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    public void StopWandering()
    {
        if (wanderCoroutine != null)
        {
            StopCoroutine(wanderCoroutine);
            wanderCoroutine = null;
        }
    }

    private IEnumerator WanderRoutine()
    {
        if (playerControl == null)
        {
            Debug.Log("No player control in wander routine");

        }
        while (!playerControl.IsControlled)
        {
            targeting.SetFollowingTarget();
            move.RollDice();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    public void SetDirectionChangeInterval(float interval)
    {
        directionChangeInterval = interval;
    }
}
