using NUnit.Framework.Constraints;
using UnityEngine;

public class SickCollisions: CircleCollisions
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundaries"))
        {
            gameObject.GetComponent<CircleMove>().TryKill();
        }

    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MainBase"))
        {
            CollectSick();
        }
        base.OnCollisionEnter2D(collision);
    }
    private void CollectSick()
    {
        CurrencyManager.Instance.AddBrains(gameObject.GetComponent<CircleMove>().PartsValue);
        CurrencyManager.Instance.AddBodies(gameObject.GetComponent<CircleMove>().PartsValue);
        gameObject.SetActive(false);
    }
}
