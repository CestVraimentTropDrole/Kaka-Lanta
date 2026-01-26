using UnityEngine;
using System.Collections.Generic;

public class PlayersManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoints;
    public int numberPlayers;

    public int currentPlayer = 0;

    public Color[] playerColors = new Color[] 
    { 
        Color.red,      // Joueur 1
        Color.blue,     // Joueur 2
        Color.green,    // Joueur 3
        Color.yellow    // Joueur 4
    };

    private List<GameObject> players = new List<GameObject>(); // Liste pour stocker les joueurs

    public void Start()
    {
        if (GameData.instance != null) {
            numberPlayers = GameData.instance.numberPlayers;
            Debug.Log("Nombre de joueurs récupéré : " + numberPlayers);
        }

        for (int i = 0; i < numberPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoints.position, Quaternion.identity);
            player.name = "Player " + (i + 1);

            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null) { movement.playerNumber = i + 1;}

            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            sprite.color = playerColors[i];

            players.Add(player);    // Ajoute le joueur à la liste 

            Debug.Log("Joueur " + (i+1) + " créé avec succès en " + spawnPoints.position);
        }

        SetActivePlayer();
    }

    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextPlayer();
        }
    }

    private void SetActivePlayer()
    {
        for (int i=0; i<players.Count; i++)
        {
            players[i].SetActive(i == currentPlayer);   // Active uniquement le joueur actuel
        }

        Debug.Log("Joueur actif : " + (currentPlayer + 1));
    }

    public void NextPlayer()
    {
        currentPlayer++;
        if (currentPlayer >= numberPlayers)
        {
            currentPlayer = 0; // Retourne au premier joueur
        }
        SetActivePlayer();
    }

    public GameObject GetActivePlayer()
    {
        if (currentPlayer >= 0 && currentPlayer < players.Count) { return players[currentPlayer]; }
        return null;
    }
}
