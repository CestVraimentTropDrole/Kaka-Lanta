using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject settingsWindow;
    public Button firstButton;
    public TMP_Dropdown playerDropdown;

    void Start()
    {
        settingsWindow.SetActive(false);

        if (playerDropdown != null)
        {
            playerDropdown.ClearOptions();
            playerDropdown.AddOptions(new System.Collections.Generic.List<string> { "2 joueurs", "3 joueurs", "4 joueurs" });
            playerDropdown.value = 0;
        }
    }

    public void StartGame()
    {
        int playerCount = 2;

        if (playerDropdown != null) {
            playerCount = playerDropdown.value + 2;
        }

        if (GameData.instance != null) {
            GameData.instance.SetNumberOfPlayers(playerCount);
        }

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
