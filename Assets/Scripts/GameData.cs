using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [Header("Configuration de la partie")]
    public int numberPlayers = 2; // Nombre de joueurs
    
    [Header("Progression")]
    public int currentRound = 1; // Jour actuel
    
    [Header("Récompenses mini-jeu")]
    public int minigameWinnerPlayerIndex = -1; // Index du joueur gagnant (-1 = personne)
    
    [Header("Données des joueurs")]
    public List<GameObject> listPlayers = new List<GameObject>(); // Liste pour stocker les joueurs
    public List<PlayerSaveData> playersSaveData = new List<PlayerSaveData>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("GameData dupliqué détruit");
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("✅ GameData créé et persistant");
    }

    public void SetNumberOfPlayers(int number)
    {
        numberPlayers = Mathf.Clamp(number, 2, 4);
        Debug.Log("Number of players set to: " + numberPlayers);
    }

    public void SetCurrentRound(int round)
    {
        currentRound = round;
        Debug.Log("Current round set to: " + currentRound);
    }

    public void SavePlayers(List<GameObject> players)
    {
        listPlayers = players;
        playersSaveData.Clear();
        
        foreach (GameObject player in players)
        {
            PlayerSaveData data = new PlayerSaveData();
            
            PlayerHunger hunger = player.GetComponent<PlayerHunger>();
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            
            if (hunger != null)
            {
                data.currentHunger = hunger.currentHunger;
                data.maxHunger = hunger.maxHunger;
            }
            
            if (health != null)
            {
                data.currentHealth = health.currentHealth;
                data.maxHealth = health.maxHealth;
            }
            
            playersSaveData.Add(data);
        }
        
        Debug.Log("💾 Données de " + players.Count + " joueurs sauvegardées");
    }

    public void LoadPlayers(List<GameObject> players)
    {
        for (int i = 0; i < players.Count && i < playersSaveData.Count; i++)
        {
            PlayerSaveData data = playersSaveData[i];
            
            PlayerHunger hunger = players[i].GetComponent<PlayerHunger>();
            PlayerHealth health = players[i].GetComponent<PlayerHealth>();
            
            if (hunger != null)
            {
                hunger.currentHunger = data.currentHunger;
                hunger.maxHunger = data.maxHunger;
            }
            
            if (health != null)
            {
                health.currentHealth = data.currentHealth;
                health.maxHealth = data.maxHealth;
            }
        }
        
        Debug.Log("📂 Données de " + players.Count + " joueurs chargées");
    }

    public void ResetMinigameRewards()
    {
        minigameWinnerPlayerIndex = -1;
    }
}