using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject settingsWindow;
    public Button firstButton;
    public TMP_Dropdown playerDropdown;

    void Start()
    {
        settingsWindow.SetActive(false);    // Cache la fenêtre des paramètres

        if (playerDropdown != null) // Génère les options du dropdown du nombre de joueurs
        {
            playerDropdown.ClearOptions();
            playerDropdown.AddOptions(new System.Collections.Generic.List<string> { "2 joueurs", "3 joueurs", "4 joueurs" });
            playerDropdown.value = 0;
        }
    }

    public void StartGame()
    {
        int playerCount = 2;    // Valeur par défaut

        if (playerDropdown != null) {
            playerCount = playerDropdown.value + 2;
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
