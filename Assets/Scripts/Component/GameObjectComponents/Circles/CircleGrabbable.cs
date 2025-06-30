using System.Collections.Generic;
using UnityEngine;

public class CircleGrabbable : MonoBehaviour
{
    private List<GameObject> held = new();
    [SerializeField] private int maxHeld = 2;

    public bool CanGrab => held.Count < maxHeld;
    public List<GameObject> GetHeld() => held;

    public void Grab(GameObject target)
    {
        if (target == null) return;

        CircleMove targetMove = target.GetComponent<CircleMove>();
        if (targetMove == null) return;

        if (!CanGrab || held.Contains(target) || !targetMove.Grabbable.CanGrab)
            return;

        held.Add(target);

        if (!targetMove.Grabbable.held.Contains(gameObject))
            targetMove.Grabbable.held.Add(gameObject);
    }

    public void Release(GameObject target)
    {
        if (target == null) return;

        held.Remove(target);

        CircleMove targetMove = target.GetComponent<CircleMove>();
        if (targetMove != null)
            targetMove.Grabbable.held.Remove(gameObject);
    }

    public void ResetGrabs()
    {
        List<GameObject> heldCopy = new(held);
        foreach (GameObject go in heldCopy)
        {
            Release(go);
        }
    }
}
