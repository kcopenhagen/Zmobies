using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceInspectorZone : MonoBehaviour
{

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void CheckHover(DieMove die, PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, eventData.position, eventData.pressEventCamera))
        {
            die.ShowInfo();
        }
        else
        {
            die.HideInfo();
        }
    }    
}
