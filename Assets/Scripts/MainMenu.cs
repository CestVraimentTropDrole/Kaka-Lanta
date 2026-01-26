using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject settingsWindow;
    public Button firstButton;

    void Start()
    {
        settingsWindow.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void SettingsButton()
    {
        settingsWindow.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
