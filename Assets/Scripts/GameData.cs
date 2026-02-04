using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static GameData instance;    // Déclaration en Instance
    public int numberPlayers = 2;   // Nombre de joueurs
    public int currentRound = 1;    // Tour actuel
    public List<GameObject> listPlayers = new List<GameObject>();  // Liste pour stocker les joueurs

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Il y a plus d'une instance GameData dans la scène");
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
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
        Debug.Log("Players saved. Total players: " + listPlayers.Count);
    }
}