using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 0.002f;
    [SerializeField] private float panSpeed = 0.01f;
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 30f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandleTouchZoomAndPan();
#if UNITY_EDITOR
        HandleScrollZoom();
#endif
    }

    void HandleTouchZoomAndPan()
    {
        if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2)
            return;

        var touch0 = Touchscreen.current.touches[0];
        var touch1 = Touchscreen.current.touches[1];

        if (!touch0.press.isPressed || !touch1.press.isPressed)
            return;

        Vector2 currTouch0 = touch0.position.ReadValue();
        Vector2 currTouch1 = touch1.position.ReadValue();
        Vector2 prevTouch0 = currTouch0 - touch0.delta.ReadValue();
        Vector2 prevTouch1 = currTouch1 - touch1.delta.ReadValue();

        // Zoom
        float prevDist = Vector2.Distance(prevTouch0, prevTouch1);
        float currDist = Vector2.Distance(currTouch0, currTouch1);
        float delta = prevDist - currDist;

        cam.orthographicSize += delta * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        // Pan
        Vector2 avgDelta = (touch0.delta.ReadValue() + touch1.delta.ReadValue()) / 2f;
        Vector3 pan = new Vector3(-avgDelta.x * panSpeed, -avgDelta.y * panSpeed, 0);

        // Adjust for camera zoom level
        pan *= cam.orthographicSize / 5f;

        cam.transform.position += pan;
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
