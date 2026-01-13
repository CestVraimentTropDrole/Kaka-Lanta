using UnityEngine;
using System.Collections.Generic;

public class PlayersManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoints;
    public int numberPlayers;

    public Color[] playerColors = new Color[] 
    { 
        Color.red,      // Joueur 1
        Color.blue,     // Joueur 2
        Color.green,    // Joueur 3
        Color.yellow    // Joueur 4
    };

    public void Start()
    {
        for (int i = 0; i < numberPlayers; i++)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoints.position, Quaternion.identity);
            player.name = "Player " + (i + 1);

            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            if (movement != null) { movement.playerNumber = i + 1;}

            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            sprite.color = playerColors[i];

            Debug.Log("Joueur " + (i+1) + " créé avec succès en " + spawnPoints.position);
        }
    }

    
    public void Update()
    {
        
    }
}
