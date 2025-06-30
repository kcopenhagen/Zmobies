using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropTarget : MonoBehaviour, IDropHandler
{
    [SerializeField] private string source;
    public string Source => source;
    public void OnDrop(PointerEventData eventData)
    {

    }
}
