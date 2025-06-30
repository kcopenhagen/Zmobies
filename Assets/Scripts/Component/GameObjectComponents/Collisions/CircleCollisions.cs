using UnityEngine;

public class CircleCollisions : MonoBehaviour
{
    private int storedDemolishes;

    protected virtual void Start()
    {
        storedDemolishes = 0;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (RemoveDemolish(1))
            {
                Destroy(collision.gameObject);
            }
        }

        CircleGrabbable cm = gameObject.GetComponent<CircleGrabbable>();
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            if (cm != null)
            {
                cm.Grab(collision.gameObject);
            }
        }
    }
  
    public void AddDemolish(int value)
    {
        storedDemolishes += value;
    }
    public bool RemoveDemolish(int value)
    {
        if (storedDemolishes - value < 0)
        {
            return false;
        }
        else
        {
            storedDemolishes -= value;
            return true;
        }
    }
}
