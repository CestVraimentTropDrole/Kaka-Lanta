using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private TMP_Text saveStatusText;
    
    private bool isPaused = false;

    public static PauseMenu instance;

    void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    void Update()
    {
        // Appuyer sur Échap pour ouvrir/fermer le menu pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Fige le jeu
        
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Reprend le jeu
        
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void SaveGame()
    {
        if (SaveSystem.instance != null)
        {
            SaveSystem.instance.SaveGame();
            
            if (saveStatusText != null)
            {
                saveStatusText.text = "Partie sauvegardée !";
                saveStatusText.color = Color.green;
                Invoke(nameof(ClearStatusText), 2f);
            }
        }
        else
        {
            
            if (saveStatusText != null)
            {
                saveStatusText.text = "❌ Erreur de sauvegarde";
                saveStatusText.color = Color.red;
            }
        }
    }

    public void LoadGame()
    {
        if (SaveSystem.instance != null)
        {
            // Reprendre le jeu avant de charger
            Time.timeScale = 1f;
            
            SaveSystem.instance.LoadGame();
            
            // Fermer le menu pause après chargement
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            
            isPaused = false;
        }
        else
        {
            return;
        }
    }

    public void ReturnToMainMenu()
    {
        // Demander confirmation avant de quitter
        Time.timeScale = 1f; // Remettre le temps normal
        SceneManager.LoadScene("MainMenu"); // Changez selon le nom de votre scène de menu
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void ClearStatusText()
    {
        if (saveStatusText != null)
            saveStatusText.text = "";
    }
}