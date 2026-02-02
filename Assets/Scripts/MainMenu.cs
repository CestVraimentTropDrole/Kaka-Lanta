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
    
    [Header("Sauvegarde")]
    public Button loadGameButton; // ⚠️ NOUVEAU : Bouton "Charger Partie"
    public GameObject loadButtonObject; // ⚠️ NOUVEAU : Le GameObject entier (si vous voulez le cacher complètement)

    void Start()
    {
        settingsWindow.SetActive(false);

        if (playerDropdown != null)
        {
            playerDropdown.ClearOptions();
            playerDropdown.AddOptions(new System.Collections.Generic.List<string> { "2 joueurs", "3 joueurs", "4 joueurs" });
            playerDropdown.value = 0;
        }
        
        // ⚠️ NOUVEAU : Vérifier si une sauvegarde existe
        CheckSaveFile();
    }

    private void CheckSaveFile()
    {
        bool hasSave = false;
        
        if (SaveSystem.instance != null)
        {
            hasSave = SaveSystem.instance.HasSaveFile();
        }
        
        // Activer/Désactiver le bouton Charger selon si une sauvegarde existe
        if (loadGameButton != null)
        {
            loadGameButton.interactable = hasSave;
            
            // Changer la couleur pour montrer qu'il est désactivé
            if (!hasSave)
            {
                var colors = loadGameButton.colors;
                colors.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                loadGameButton.colors = colors;
            }
        }
        
        // Optionnel : Cacher complètement le bouton s'il n'y a pas de sauvegarde
        if (loadButtonObject != null)
        {
            loadButtonObject.SetActive(hasSave);
        }
        
        Debug.Log(hasSave ? "💾 Sauvegarde détectée" : "⚠️ Aucune sauvegarde trouvée");
    }

    public void StartGame()
    {
        int playerCount = 2;

        if (playerDropdown != null) {
            playerCount = playerDropdown.value + 2;
        }

        if (GameData.instance != null) {
            GameData.instance.SetNumberOfPlayers(playerCount);
            // Réinitialiser pour une nouvelle partie
            GameData.instance.currentRound = 1;
            GameData.instance.playersSaveData.Clear();
            GameData.instance.ResetMinigameRewards();
        }

        SceneManager.LoadScene(levelToLoad);
        Debug.Log($"🎮 Nouvelle partie lancée avec {playerCount} joueurs");
    }

    public void LoadGame()
    {
        if (SaveSystem.instance != null)
        {
            if (SaveSystem.instance.HasSaveFile())
            {
                SaveSystem.instance.LoadGame();
                Debug.Log("📂 Chargement de la partie...");
            }
            else
            {
                Debug.LogWarning("⚠️ Aucune sauvegarde à charger !");
            }
        }
        else
        {
            Debug.LogError("❌ SaveSystem introuvable !");
        }
    }

    public void DeleteSave()
    {
        if (SaveSystem.instance != null)
        {
            SaveSystem.instance.DeleteSave();
            CheckSaveFile(); // Mettre à jour l'UI
            Debug.Log("🗑️ Sauvegarde supprimée");
        }
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
        Debug.Log("👋 Fermeture du jeu");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}