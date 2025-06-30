
using System.Collections.Generic;
using UnityEngine;


public class DieManager : MonoBehaviour
{
    public static DieManager Instance;
    [SerializeField] GameObject obstaclePrefab;
    private readonly float speedFactor = 1f / 4f;
    private readonly float wandDirFactor = 2f * Mathf.PI / 12f; // 12 = 2*pi etc. To convert from clock directions to radians.
    
    private readonly float continuousFsScale = 0.1f;
    private readonly float maxHoldLengthFactor = 2f;

    private float newSpeed;
    private Vector2 moveDir;
    private Vector2 newDir;
    private float refDir;
    private float m = 1f;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public (float newSpeed, Vector2 newDir) RollAllDice(CircleMove circle)
    {
        newDir = Vector2.zero;
        newSpeed = 0f;

        moveDir = circle.GetMoveDirection();
        refDir = Mathf.Atan2(moveDir.y, moveDir.x);

        foreach (DieSettings die in circle.MyDice)
            RollDie(die, circle);

        if (newDir != Vector2.zero)
        {
            newDir = newDir.normalized;
        }
        else
        {
            newDir = moveDir.normalized;
        }
        return (newSpeed, newDir);
    }

    public void RollContinuousDice(CircleMove circle)
    {
        Vector2 cv = circle.GetSpeed() * circle.GetMoveDirection();
        foreach (DieSettings die in circle.MyDice)
        {
            switch (die.type)
            {
                case "Holding":
                    cv += RollHoldingDie(die,circle); break;
            }
        }
        circle.GetComponent<Rigidbody2D>().linearVelocity = cv;
    }

    private void RollDie(DieSettings die, CircleMove circle)
    {
        switch (die.type)
        {
            case "Speed":
                RollSpeedDie(die);
                break;
            case "Wandering":
                RollWanderingDie(die);
                break;
            case "Homing":
            case "Following":
                RollHomingDie(die, circle);
                break;
            case "Fleeing":
            case "Evading":
                RollFleeingDie(die, circle);
                break;
            case "Holding":
                RollHoldingDieThought(die, circle);
                break;
            case "Hoarding":
                RollHoardingDie(die, circle);
                break;
            case "Killing":
                RollKillingDie(die, circle);
                break;
            case "Demolishing":
                RollDemolishingDie(die, circle);
                break;
            case "Building":
                RollBuildingDie(die, circle);
                break;
            case "ExtraLife":
                RollExtraLifeDie(die, circle);
                break;
            default:
                break;
        }
    }

    private void RollSpeedDie(DieSettings die)
    {
        newSpeed += speedFactor * die.sideValues[Random.Range(0, die.sideValues.Length)];
        return;
    }
    private void RollWanderingDie(DieSettings die)
    {
        float value = die.sideValues[Random.Range(0, die.sideValues.Length)];

        Vector2 wandDir = new(Mathf.Cos(value * wandDirFactor + refDir), Mathf.Sin(value * wandDirFactor + refDir));
        newDir += die.size * wandDir;
        return;
    }
    private void RollHomingDie(DieSettings die, CircleMove circle)
    {
        float value = die.sideValues[Random.Range(0, die.sideValues.Length)];
        if (!circle.TryGetComponent<Rigidbody2D>(out var rb))
        {
            Debug.Log("No rigid body found on fleeing circle");
        }
        else
        {
            newDir += value * die.size * (die.target - rb.position);
        }
    }
    private void RollFleeingDie(DieSettings die, CircleMove circle)
    {
        float value = die.sideValues[Random.Range(0, die.sideValues.Length)];

        if (!circle.TryGetComponent<Rigidbody2D>(out var rb))
        {
            Debug.Log("No rigid body found on fleeing circle");
        }
        else
        {
            newDir -= value * die.size * (die.target - rb.position);
        }
    }
    private Vector2 RollHoldingDie(DieSettings die, CircleMove circle)
    {
        CircleMove curCircle = circle.GetComponent<CircleMove>(); 
        float r0 = curCircle.GetR0();
        Vector2 cv = Vector2.zero;
        List<GameObject> heldCopy = new List<GameObject>(circle.GetComponent<CircleGrabbable>().GetHeld());
        Rigidbody2D rb = circle.GetComponent<Rigidbody2D>();
        foreach (GameObject held in heldCopy)
        {
            Rigidbody2D rb2 = held.GetComponent<Rigidbody2D>();
            Vector2 dR = (rb2.position - rb.position);
            Vector2 springF = die.size * (dR.magnitude - r0) * dR.normalized;
            if (dR.magnitude > maxHoldLengthFactor * r0)
            {
                circle.GetComponent<CircleGrabbable>().Release(held);
            }
            cv += continuousFsScale * springF / m;
        }
        return cv;
    }

    private void RollHoldingDieThought(DieSettings die, CircleMove circle)
    {
        float value = die.sideValues[Random.Range(0, die.sideValues.Length)];
        if (value == 0)
        {
            circle.GetComponent<CircleGrabbable>().ResetGrabs();
        }
    }

    private void RollHoardingDie(DieSettings die, CircleMove circle)
    {
        float value = die.sideValues[Random.Range(0, die.sideValues.Length)];
        Vector2 hoardDir = Vector2.zero;
        if (value != 0)
        {
            List<CircleMove> allCircles = new();
            if (circle.GetComponent<HealthyMove>() != null)
                allCircles = HealthyMove.All;
            else if (circle.GetComponent<ZmobMove>() != null)
                allCircles = ZmobMove.All;
            else if (circle.GetComponent<SickMove>() != null)
                allCircles = SickMove.All;
            else
                return;

            foreach (CircleMove circ in allCircles)
            {
                float dR = (circ.transform.position - circle.transform.position).magnitude;
                if (dR < value)
                    hoardDir += circ.GetMoveDirection();
            }
        }
        newDir += die.size * hoardDir.normalized;
    }
    private void RollKillingDie(DieSettings die, CircleMove circle)
    {
        int value = (int)die.sideValues[Random.Range(0, die.sideValues.Length)];
        foreach (ZmobMove zmob in ZmobMove.All)
        {
            float dR = ((Vector2)zmob.GetComponent<Transform>().position
                - (Vector2)circle.GetComponent<Transform>().position).magnitude;
            if (dR < value)
            {
                zmob.TryKill();
                return;
            }
        }
    }
    private void RollDemolishingDie(DieSettings die, CircleMove circle)
    {
        int value = (int)die.sideValues[Random.Range(0, die.sideValues.Length)];
        circle.GetComponent<CircleCollisions>().AddDemolish(value);
    }
    private void RollBuildingDie(DieSettings die, CircleMove circle)
    {
        int value = (int)die.sideValues[Random.Range(0, die.sideValues.Length)];
        if (value > 0)
        {
            float dropDistance = (float)value;
            Vector2 direction = circle.GetMoveDirection();
            Vector2 rotatedDir = Quaternion.Euler(0,0,refDir) * direction;

            Vector2 spawnPos = (Vector2)circle.transform.position + rotatedDir * dropDistance;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, rotatedDir);
            Instantiate(obstaclePrefab, spawnPos, rotation);
        }
    }
    private void RollExtraLifeDie(DieSettings die, CircleMove circle)
    {
        int value = (int)die.sideValues[Random.Range(0, die.sideValues.Length)];
        circle.Lives += value;
    }
}
