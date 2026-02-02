using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [Header("Game Info")]
    public int numberPlayers = 2;
    public int currentRound = 1;

    [Header("Players Save Data")]
    public List<PlayerSaveData> playersSaveData = new List<PlayerSaveData>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetNumberOfPlayers(int number)
    {
        numberPlayers = Mathf.Clamp(number, 2, 4);
        Debug.Log("🎮 Nombre de joueurs : " + numberPlayers);
    }

    public void SetCurrentRound(int round)
    {
        currentRound = round;
        Debug.Log("🔄 Tour actuel : " + currentRound);
    }

    // 🔹 SAUVEGARDE DES JOUEURS
    public void SavePlayers(List<GameObject> players)
    {
        playersSaveData.Clear();

        foreach (GameObject player in players)
        {
            PlayerMovement movement = player.GetComponent<PlayerMovement>();

            PlayerSaveData data = new PlayerSaveData
            {
                playerNumber = movement.playerNumber,
                position = player.transform.position
            };

            playersSaveData.Add(data);
        }

        Debug.Log("💾 Joueurs sauvegardés : " + playersSaveData.Count);
    }

    // 🔹 CHARGEMENT DES JOUEURS
    public void LoadPlayers(List<GameObject> players)
    {
        for (int i = 0; i < playersSaveData.Count && i < players.Count; i++)
        {
            players[i].transform.position = playersSaveData[i].position;
        }

        Debug.Log("📂 Joueurs restaurés");
    }

    public void ResetMinigameRewards()
    {
        // Si tu en as besoin, sinon laisse vide
    }
}
