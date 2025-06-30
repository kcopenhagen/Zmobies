using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class DoorPlayerDragging : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Camera mainCamera;

    private bool isPlayerControlled = false;
    [SerializeField] private bool isDraggable = true;
    public bool IsDraggable => isDraggable;
    public static List<DoorPlayerDragging> All { get; } = new List<DoorPlayerDragging>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        mainCamera = Camera.main;
        All.Add(this);
    }

    private void Update()
    {
        if (!isPlayerControlled)
        {
            rb.angularVelocity = 0;
            rb.linearVelocity = Vector2.zero;
        }    
    }

    public void StartControl()
    {
        isPlayerControlled = true;
    }

    public void StopControl()
    {
        isPlayerControlled = false;
    }

    public void DragTo(Vector2 screenPosition)
    {
        if (!isPlayerControlled || mainCamera == null) return;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        worldPos.z = 0f;
        transform.position = worldPos;
        rb.linearVelocity = Vector2.zero;
    }

    public bool IsTouched(Vector2 screenPosition)
    {
        if (col == null || mainCamera == null) return false;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPosition);
        return col.OverlapPoint(worldPos);
    }

    public bool IsControlled => isPlayerControlled;
}
