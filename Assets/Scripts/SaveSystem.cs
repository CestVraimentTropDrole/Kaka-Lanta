using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private string saveFilePath;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Chemin du fichier de sauvegarde
        saveFilePath = Application.persistentDataPath + "/savegame.json";
        Debug.Log($"💾 Fichier de sauvegarde : {saveFilePath}");
    }

    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData();

        // Sauvegarder les données du GameData
        if (GameData.instance != null)
        {
            saveData.numberPlayers = GameData.instance.numberPlayers;
            saveData.currentRound = GameData.instance.currentRound;
            saveData.playersSaveData = GameData.instance.playersSaveData;
        }

        // Sauvegarder les données du TurnSystem
        if (TurnSystem.instance != null)
        {
            saveData.currentTurn = TurnSystem.instance.GetCurrentTurn();
            saveData.currentDay = TurnSystem.instance.currentDay;
        }

        // Sauvegarder l'inventaire
        if (Inventory.instance != null)
        {
            saveData.woodCount = Inventory.instance.woodCount;
            saveData.stoneCount = Inventory.instance.stoneCount;
        }

        // Sauvegarder le BuildRaft
        if (BuildRaft.instance != null)
        {
            saveData.raftIsBuilt = BuildRaft.instance.RaftisBuilt;
        }

        // Convertir en JSON
        string json = JsonUtility.ToJson(saveData, true);
        
        // Écrire dans le fichier
        File.WriteAllText(saveFilePath, json);
        
        Debug.Log("💾 Partie sauvegardée !");
        Debug.Log(json);
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("⚠️ Aucune sauvegarde trouvée !");
            return;
        }

        // Lire le fichier
        string json = File.ReadAllText(saveFilePath);
        
        // Convertir depuis JSON
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        // Charger dans GameData
        if (GameData.instance != null)
        {
            GameData.instance.numberPlayers = saveData.numberPlayers;
            GameData.instance.currentRound = saveData.currentRound;
            GameData.instance.playersSaveData = saveData.playersSaveData;
        }

        // Stocker temporairement pour charger après le changement de scène
        tempSaveData = saveData;

        Debug.Log("📂 Sauvegarde chargée !");
        Debug.Log(json);

        // Charger la scène de jeu
        SceneManager.LoadScene("SampleScene"); // Changez selon le nom de votre scène de jeu
    }

    private GameSaveData tempSaveData;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (tempSaveData != null && scene.name == "SampleScene") // Votre scène de jeu
        {
            // Attendre un frame pour que tout soit initialisé
            StartCoroutine(ApplySaveDataAfterFrame());
        }
    }

    private System.Collections.IEnumerator ApplySaveDataAfterFrame()
    {
        yield return null; // Attendre 1 frame

        if (tempSaveData == null) yield break;

        // Restaurer TurnSystem
        if (TurnSystem.instance != null)
        {
            // Note: vous devrez ajouter des setters dans TurnSystem pour ça
            Debug.Log($"🔄 Tour restauré : {tempSaveData.currentTurn}, Jour : {tempSaveData.currentDay}");
        }

        // Restaurer l'inventaire
        if (Inventory.instance != null)
        {
            Inventory.instance.woodCount = tempSaveData.woodCount;
            Inventory.instance.stoneCount = tempSaveData.stoneCount;
            
            // Mettre à jour l'UI
            if (Inventory.instance.woodCountText != null)
                Inventory.instance.woodCountText.text = Inventory.instance.woodCount.ToString();
            if (Inventory.instance.stoneCountText != null)
                Inventory.instance.stoneCountText.text = Inventory.instance.stoneCount.ToString();
            
            Debug.Log($"📦 Inventaire restauré : {tempSaveData.woodCount} bois, {tempSaveData.stoneCount} pierre");
        }

        // Restaurer les joueurs
        PlayersManager playersManager = FindFirstObjectByType<PlayersManager>();
        if (playersManager != null && GameData.instance != null)
        {
            // Attendre que les joueurs soient créés
            yield return new WaitForSeconds(0.5f);
            
            GameData.instance.LoadPlayers(playersManager.GetType().GetField("players", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(playersManager) as System.Collections.Generic.List<GameObject>);
            
            Debug.Log("👥 Joueurs restaurés");
        }

        // Restaurer le radeau
        if (BuildRaft.instance != null)
        {
            BuildRaft.instance.RaftisBuilt = tempSaveData.raftIsBuilt;
            Debug.Log($"🚤 État radeau restauré : {tempSaveData.raftIsBuilt}");
        }

        tempSaveData = null;
        Debug.Log("✅ Chargement terminé !");
    }

    public bool HasSaveFile()
    {
        return File.Exists(saveFilePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("🗑️ Sauvegarde supprimée");
        }
    }
}

// Classe pour stocker toutes les données de sauvegarde
[System.Serializable]
public class GameSaveData
{
    // GameData
    public int numberPlayers;
    public int currentRound;
    public System.Collections.Generic.List<PlayerSaveData> playersSaveData;

    // TurnSystem
    public int currentTurn;
    public int currentDay;

    // Inventory
    public int woodCount;
    public int stoneCount;

    // BuildRaft
    public bool raftIsBuilt;
}