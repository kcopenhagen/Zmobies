using UnityEngine;
using UnityEngine.UI;

public class ThoughtPanelManager : MonoBehaviour
{
    [SerializeField] private string panelName;
    void Start()
    {
        Image im = GetComponent<Image>();
        if (im == null )
        {
            Debug.LogWarning("No image component on panel");
            return;
        }
        if (ColorManager.Instance == null)
        {
            Debug.LogError("No color manager");
            return;
        }
        im.color = ColorManager.Instance.SelectPanelColor(panelName);
    }

}
