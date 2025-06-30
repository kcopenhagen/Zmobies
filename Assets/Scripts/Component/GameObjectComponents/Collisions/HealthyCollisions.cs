using NUnit.Framework.Constraints;
using UnityEngine;

[RequireComponent(typeof(HealthyMove))]
public class HealthyCollisions : CircleCollisions
{

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MainBase"))
        {
            CollectHealthy(1);
        }
        base.OnCollisionEnter2D(collision);
    }
    private void CollectHealthy(int value)
    {
        CurrencyManager.Instance.SpendBodies(value);
        gameObject.GetComponent<HealthyMove>().RunAway(true);
    }
}
