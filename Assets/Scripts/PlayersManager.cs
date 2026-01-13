using UnityEngine;

public class PlayersManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform spawnPoints;
    public int numberPlayers;

    public void Start()
    {
        GameObject player = Instantiate(playerPrefab, spawnPoints.position, Quaternion.identity);
        player.name = "Player " + 1;
    }

    
    public void Update()
    {
        
    }
}
