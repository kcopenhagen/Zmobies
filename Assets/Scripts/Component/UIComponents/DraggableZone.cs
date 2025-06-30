using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class DraggableZone : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private bool isDragging = false;
    private Vector2 originalPosition;

    [SerializeField] private Vector2 boxSize = new Vector2(200, 200);

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        isDragging = true;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;
        rectTransform.position = originalPosition;
        List<GameObject> allDice = UIManager.Instance.GetComponent<DieSpawner>().GetUsedDice();
        foreach (GameObject dice in allDice)
        {
            if (dice != null)
            {
                dice.GetComponent<DieMove>().HideInfo();
            }
        }
        SpawnDieButtonScript[] allDieSpawnButtons = FindObjectsByType<SpawnDieButtonScript>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (SpawnDieButtonScript button in allDieSpawnButtons)
        {
            if (button != null)
            {
                button.HideInfo();
            }
        }
        GotoDesignButtonManager[] allDesignButtons = FindObjectsByType<GotoDesignButtonManager>(
            FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (GotoDesignButtonManager button in allDesignButtons)
        {
            if (button != null)
            {
                button.HideInfo();
            }
        }
    }

    private void Update()
    {
        if (!isDragging) return;

        var uiHits = GetUIObjectsUnderPointer();
        foreach (var obj in uiHits)
        {
            if (obj.TryGetComponent(out GotoDesignButtonManager dbutton))
            {
                dbutton.ShowInfo();
            }
            if (obj.TryGetComponent(out SpawnDieButtonScript spawnButton))
            {
                spawnButton.ShowInfo();
            }
            if (obj.TryGetComponent(out DieMove die))
            {
                die.ShowInfo();
            }
        }
    }


    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position, boxSize);
    //}

    public List<GameObject> GetUIObjectsUnderPointer()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Pointer.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        List<GameObject> hitObjects = new List<GameObject>();
        foreach (RaycastResult result in results)
        {
            hitObjects.Add(result.gameObject);
        }

        return hitObjects;
    }
}
