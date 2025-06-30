using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 0.002f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 30f;

    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouchZoom();
#if UNITY_EDITOR
        HandleScrollZoom();
#endif
    }

    void HandleTouchZoom()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2)
            return;

        var touch0 = Touchscreen.current.touches[0];
        var touch1 = Touchscreen.current.touches[1];

        if (!touch0.press.isPressed || !touch1.press.isPressed)
            return;

        Vector2 prevTouch0 = touch0.startPosition.ReadValue();
        Vector2 prevTouch1 = touch1.startPosition.ReadValue();
        Vector2 currTouch0 = touch0.position.ReadValue();
        Vector2 currTouch1 = touch1.position.ReadValue();

        float prevDist = Vector2.Distance(prevTouch0, prevTouch1);
        float currDist = Vector2.Distance(currTouch0, currTouch1);
        float delta = prevDist - currDist;

        cam.orthographicSize += delta * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    void HandleScrollZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            cam.orthographicSize -= scroll * zoomSpeed * 100f;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
