using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CirclePlayerControl))]
public class CircleMove : MonoBehaviour
{
    [SerializeField] protected float deathChance = 0.05f;
    [SerializeField] protected List<DieSettings> myDice = new();
    [SerializeField] protected Color reviveColor = Color.white;

    protected Rigidbody2D rb;
    protected CirclePlayerControl playerControl;
    protected CircleGrabbable grabbable;
    protected CircleWander wander;
    protected CircleTargeting targeting;
    protected SpriteRenderer sr;

    [SerializeField] protected float speed = 0f;
    protected Vector2 moveDirection;
    protected float r0;
    public int Lives { get; set; } = 0;
    public int PartsValue { get; set; } = 1;
    private int blinkDuration = 1;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControl = GetComponent<CirclePlayerControl>();
        grabbable = GetComponent<CircleGrabbable>();
        wander = GetComponent<CircleWander>();
        targeting = GetComponent<CircleTargeting>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnEnable()
    {
        moveDirection = rb.position.normalized;
        rb.linearVelocity = Vector2.zero;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null) r0 = sr.bounds.size.x;
    }

    protected virtual void OnDisable()
    {
        wander.StopWandering();
        grabbable.ResetGrabs();
        myDice.Clear();
        speed = 0f;
    }
    public bool TryKill()
    {
        if (Lives > 0)
        {
            Lives--;
            ReviveBlink();
            return false;
        }
        else
        {
            gameObject.SetActive(false);
            return true;
        }
    }
    private void ReviveBlink()
    {
        StartCoroutine(BlinkCoroutine());
    }
    private IEnumerator BlinkCoroutine()
    {
        Color originalColor = sr.color;
        sr.color = reviveColor;
        yield return new WaitForSeconds(blinkDuration);
        sr.color = originalColor;
    }
    protected virtual void Update()
    {
        if (Random.Range(0f, 1f) < deathChance * Time.deltaTime)
        {
            TryKill();
        }
        if (!playerControl.IsControlled && gameObject.activeInHierarchy)
            DieManager.Instance.RollContinuousDice(this);
    }
    protected virtual void RollDiceInternal()
    {
        if (myDice == null || myDice.Count == 0) return;
        (speed, moveDirection) = DieManager.Instance.RollAllDice(this);
    }
    public void RollDice()
    {
        RollDiceInternal();
    }
    public void SetSpeed(float newSpeed) => speed = newSpeed;
    public float GetSpeed() => speed;
    public Vector2 GetMoveDirection() => moveDirection;
    public void SetMoveDirection(Vector2 direction) => moveDirection = direction.normalized;
    public float GetR0() => r0;
    public List<DieSettings> MyDice => myDice;
    public void AddDice(List<DieSettings> dice) => myDice.AddRange(dice);
    public CircleGrabbable Grabbable => grabbable;
    public CirclePlayerControl PlayerControl => playerControl;
}
