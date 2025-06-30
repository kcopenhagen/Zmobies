using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchInputHandler : MonoBehaviour
{
    private CirclePlayerControl selectedControl;
    private SpawnerPlayerControl selectedSpawnerControl;
    private DoorPlayerDragging selectedDoorPlayerDragging;
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnTouchStart;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += OnTouchEnd;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += OnTouchMove;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= OnTouchStart;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= OnTouchEnd;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove -= OnTouchMove;
    }

    void OnTouchStart(Finger finger)
    {
        Vector2 pos = finger.screenPosition;

        foreach (var obj in ZmobMove.All)
        {
            CirclePlayerControl control = obj.GetComponent<CirclePlayerControl>();
            if (control != null && control.IsTouched(pos) && control.IsDraggable)
            {
                selectedControl = control;
                selectedControl.StartControl();
                break;
            }
        }
        foreach (var obj in DoorPlayerDragging.All)
        {
            DoorPlayerDragging control = obj.GetComponent<DoorPlayerDragging>();
            if (control != null && control.IsTouched(pos) && control.IsDraggable)
            {
                selectedDoorPlayerDragging = control;
                selectedDoorPlayerDragging.StartControl();
                break;
            }
        }
    }

    void OnTouchMove(Finger finger)
    {
        if (selectedControl != null)
        {
            selectedControl.DragTo(finger.screenPosition);
        }
        if (selectedDoorPlayerDragging != null)
        {
            selectedDoorPlayerDragging.DragTo(finger.screenPosition);
        }
    }

    void OnTouchEnd(Finger finger)
    {
        Vector2 pos = finger.screenPosition;

        if (selectedControl != null)
        {
            selectedControl.StopControl();
            selectedControl = null;
        }
        if (selectedDoorPlayerDragging != null)
        {
            selectedDoorPlayerDragging.StopControl();
            selectedDoorPlayerDragging = null;
        }    

        foreach (var obj in Spawner.All)
        {
            SpawnerPlayerControl control = obj.GetComponent<SpawnerPlayerControl>();
            if (control != null && control.IsTouched(pos))
            {
                selectedSpawnerControl = control;
                selectedSpawnerControl.Touched();
                break;
            }
        }
    }
}
