using UnityEngine;
using System.Collections;

public class DampCamera2D : MonoBehaviour
{
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    private PlayersManager manager;
    
    void Start()
    {
        manager = FindFirstObjectByType<PlayersManager>();
        
        if (manager == null) { Debug.LogError("PlayersManager non trouvé !"); }
    }
    
    void Update()
    {
        // Récupère le joueur actif
        GameObject activePlayer = manager?.GetActivePlayer();
        
        if (activePlayer == null) return;
        
        // Suit le joueur actif
        Vector3 targetPosition = activePlayer.transform.position + new Vector3(0, 0, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}