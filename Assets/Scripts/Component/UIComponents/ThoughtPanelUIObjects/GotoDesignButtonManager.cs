using UnityEngine;
using UnityEngine.UI;

public class GotoDesignButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject designPanel;
    [SerializeField] private GameObject infoPanel;
    private void Awake()
    {
        GetComponent<DoorCostManager>().OnUnlockedClick += GotoDesignPanel;
    }
    [SerializeField] private string panelName;
    void Start()
    {
        Image im = GetComponent<Image>();
        if (im == null)
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

    private void GotoDesignPanel()
    {

        UIManager.Instance.OpenOnly(designPanel);
    }
    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }
    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
}
