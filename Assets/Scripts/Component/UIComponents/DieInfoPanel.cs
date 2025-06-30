using UnityEngine;
using TMPro;
public class DieInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI[] sideTexts;


    public void UpdateDieInfoPanel(DieSettings settings)
    {
        typeText.text = settings.type;
        for (int i = 0; i < sideTexts.Length && i < settings.sideValues.Length; i++)
        {
            sideTexts[i].text = settings.sideValues[i].ToString();
        }

    }
}
