using UnityEngine;

public class ZomCollisions: CircleCollisions
{
    private SickSpawner sickSpawner;

    private void Awake()
    {
        sickSpawner = GameObject.FindWithTag("GameController").GetComponent<SickSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundaries"))
        {
            gameObject.GetComponent<CircleMove>().TryKill();
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Healthy"))
        {
            Vector2 pos = collision.gameObject.transform.position;
            Vector2 dir = -(collision.transform.position - transform.position).normalized;

            if (collision.gameObject.GetComponent<CircleMove>().TryKill())
            {
                sickSpawner.SpawnSicko(pos, dir, collision.gameObject.GetComponent<CircleMove>().PartsValue);
            }
        }
    }
}
