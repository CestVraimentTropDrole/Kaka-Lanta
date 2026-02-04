using UnityEngine;
using System.Collections.Generic;

public class PlayersManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoints;
    public int numberPlayers;
    public int currentPlayer = 0;
    
    [Header("UI")]
    public HungerBar hungerBar; // ⚠️ AJOUTÉ : Référence à la HungerBar dans l'Inspector

    public Color[] playerColors = new Color[] 
    { 
        Color.red,      // Joueur 1
        Color.blue,     // Joueur 2
        Color.green,    // Joueur 3
        Color.yellow    // Joueur 4
    };

    private List<GameObject> players = new List<GameObject>();

    void Awake()
    {
        if (GameData.instance != null)
        {
            numberPlayers = GameData.instance.numberPlayers;
            Debug.Log("PlayersManager Awake() Nombre de joueurs récupéré : " + numberPlayers);
        }
    }

    public void Start()
    {
        for (int i = 0; i < numberPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoints.position, Quaternion.identity);
            player.name = "Player " + (i + 1);

            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null) { movement.playerNumber = i + 1; }

            // ⚠️ AJOUTÉ : Assigner la HungerBar à chaque joueur
            PlayerHunger hunger = player.GetComponent<PlayerHunger>();
            if (hunger != null && hungerBar != null)
            {
                hunger.hungerBar = hungerBar;
                Debug.Log($"HungerBar assignée au joueur {i + 1}");
            }

            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            sprite.color = playerColors[i];

            players.Add(player);

            Debug.Log("Joueur " + (i+1) + " créé avec succès en " + spawnPoints.position);
        }

        SetActivePlayer();
        
        // ⚠️ AJOUTÉ : Initialiser la barre avec le premier joueur
        UpdateHungerBarForActivePlayer();
    }

    public void Update()
    {
    }

    private void SetActivePlayer()
    {
        for (int i=0; i<players.Count; i++)
        {
            players[i].SetActive(i == currentPlayer);
        }

        Debug.Log("Joueur actif : " + (currentPlayer + 1));
        
        // ⚠️ AJOUTÉ : Mettre à jour la barre quand on change de joueur
        UpdateHungerBarForActivePlayer();
    }

    public void NextPlayer()
    {
        currentPlayer++;
        if (currentPlayer >= numberPlayers)
        {
            currentPlayer = 0;
        }
        SetActivePlayer();
    }

    public GameObject GetActivePlayer()
    {
        if (currentPlayer >= 0 && currentPlayer < players.Count) 
        { 
            return players[currentPlayer]; 
        }
        return null;
    }

    public int GetNumberOfPlayers()
    {
        Debug.Log("PlayersManager GetNumberOfPlayers() Nombre de joueurs : " + numberPlayers);
        return numberPlayers;
    }

    public void SavePlayers()
    {
        if (GameData.instance != null)
        {
            GameData.instance.SavePlayers(players);
        }
    }
    
    // ⚠️ NOUVEAU : Appliquer la récompense du mini-jeu
    private void ApplyMinigameReward()
    {
        if (GameData.instance == null) return;
        
        int winnerIndex = GameData.instance.minigameWinnerPlayerIndex;
        
        if (winnerIndex >= 0 && winnerIndex < players.Count)
        {
            PlayerHunger hunger = players[winnerIndex].GetComponent<PlayerHunger>();
            
            if (hunger != null)
            {
                // Remplir complètement la faim du gagnant
                hunger.currentHunger = hunger.maxHunger;
                Debug.Log($"🏆 Joueur {winnerIndex + 1} récupère toute sa faim ! ({hunger.maxHunger})");
                
                // Mettre à jour la barre si c'est le joueur actif
                if (currentPlayer == winnerIndex && hungerBar != null)
                {
                    hungerBar.UpdateHungerBar(hunger.currentHunger, hunger.maxHunger);
                }
            }
            
            // Réinitialiser pour la prochaine fois
            GameData.instance.ResetMinigameRewards();
        }
        else if (winnerIndex == -1)
        {
            Debug.Log("😢 Pas de gagnant au mini-jeu - Pas de récompense");
            GameData.instance.ResetMinigameRewards();
        }
    }
    
    // ⚠️ AJOUTÉ : Méthode pour mettre à jour la barre
    private void UpdateHungerBarForActivePlayer()
    {
        if (hungerBar == null) return;
        
        GameObject activePlayer = GetActivePlayer();
        if (activePlayer == null) return;
        
        PlayerHunger hunger = activePlayer.GetComponent<PlayerHunger>();
        if (hunger != null)
        {
            hungerBar.UpdateHungerBar(hunger.currentHunger, hunger.maxHunger);
        }
    }
}