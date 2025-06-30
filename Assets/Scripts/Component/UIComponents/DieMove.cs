
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DieMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas parentCanvas;
    private Transform originalParent;
    private Vector2 originalPosition;


    [SerializeField] private DieMove dieScript;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI dieFaceTextBox;
    
    [SerializeField] private DieSettings settings;
    public DieSettings GetSettings => settings;
    private DiceInspectorZone inspectorZone;
    private DieSpawner dieSpawner;

    public void SetInspectorZone(DiceInspectorZone zone) => inspectorZone = zone;
    public void SetSettings(DieSettings newSettings)
    {
        settings = newSettings;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = eventData.position;
        originalParent = transform.parent;

        if (settings.source == "Healthy")
        {
            eventData.pointerDrag = null;
            return;
        }

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentCanvas == null)
            return;

        Vector2 moveDelta = eventData.delta / parentCanvas.scaleFactor;
        rectTransform.anchoredPosition += moveDelta;

        if (inspectorZone != null)
        {
            inspectorZone.CheckHover(dieScript, eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (settings.source == "Healthy")
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
            return;
        }

        GameObject dropTarget = eventData.pointerEnter;
        if (dropTarget != null && dropTarget.GetComponent<UIDropTarget>() != null)
        {
            if (dropTarget.GetComponent<UIDropTarget>().Source == "Trash")
                dieSpawner.TrashDie(gameObject);

            transform.SetParent(dropTarget.transform);
            this.settings.source = dropTarget.GetComponent<UIDropTarget>().Source;
            this.settings.position = rectTransform.anchoredPosition;
        }
        else
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;
        if (dieScript != null)
        {
            dieScript.HideInfo();
        }

    }

    public void OnTrash()
    {
        settings.guid = null;
    }

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }
    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }

    public void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        parentCanvas = GetComponentInParent<Canvas>();
        dieSpawner = UIManager.Instance.GetComponent<DieSpawner>();
    }
    public void UpdateDieFace()
    {
        float meanValue = settings.sideValues.Average();
        dieFaceTextBox.text = meanValue.ToString("F1");
    }
}
