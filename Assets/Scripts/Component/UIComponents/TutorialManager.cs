using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject welcomeScreen;
    [SerializeField] GameObject tutorialScreen1;
    [SerializeField] GameObject tutorialScreen2;
    [SerializeField] GameObject tutorialScreen3;

    private const string TutorialKey = "HasSeenTutorial";
    private int tutorialProgress = 0;

    void Start()
    {
        welcomeScreen.SetActive(true);
        tutorialProgress = PlayerPrefs.GetInt(TutorialKey, 0);

        if (tutorialProgress == 0)
        {
            tutorialScreen1.SetActive(true);
            tutorialScreen2.SetActive(false);
            tutorialScreen3.SetActive(false);
        }
        else if (tutorialProgress == 1)
        {
            tutorialScreen1.SetActive(false);
            tutorialScreen2.SetActive(true);
            tutorialScreen3.SetActive(false);
        }
        else if (tutorialProgress == 2)
        {
            tutorialScreen1.SetActive(false);
            tutorialScreen2.SetActive(false);
            tutorialScreen3.SetActive(true);
        }
        else
        {
            tutorialScreen1.SetActive(false);
            tutorialScreen2.SetActive(false);
            tutorialScreen3.SetActive(false);
        }
    }

    public void Tutorial1Done()
    {
        if (tutorialProgress == 0)
        {
            tutorialProgress = 1;
            PlayerPrefs.SetInt(TutorialKey, tutorialProgress);
            PlayerPrefs.Save();

            tutorialScreen1.SetActive(false);
            tutorialScreen2.SetActive(true);
        }
    }

    public void Tutorial2Done()
    {
        if (tutorialProgress == 1)
        {
            tutorialProgress = 2;
            PlayerPrefs.SetInt(TutorialKey, tutorialProgress);
            PlayerPrefs.Save();

            tutorialScreen2.SetActive(false);
            tutorialScreen3.SetActive(true);
        }
    }

    public void Tutorial3Done()
    {
        if (tutorialProgress == 2)
        {
            tutorialProgress = 3;
            PlayerPrefs.SetInt(TutorialKey, tutorialProgress);
            PlayerPrefs.Save();

            tutorialScreen3.SetActive(false);
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("HasSeenTutorial");
        PlayerPrefs.Save();
        tutorialProgress = 0;

        tutorialScreen1.SetActive(true);
        tutorialScreen2.SetActive(false);
        tutorialScreen3.SetActive(false);
    }
}
