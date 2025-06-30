using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    [SerializeField] GameObject welcomePanel;

    public void CloseWelcome()
    {
        welcomePanel.SetActive(false);
    }
}
