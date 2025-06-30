using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CirclePlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private Camera mainCamera;

    private bool isPlayerControlled = false;
    [SerializeField] private bool isDraggable = true;
    public bool IsDraggable => isDraggable;

    private void Awake()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    public void StartControl()
    {
        isPlayerControlled = true;
        rb.linearVelocity = Vector2.zero;
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
