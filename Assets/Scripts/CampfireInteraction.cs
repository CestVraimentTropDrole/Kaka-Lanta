using UnityEngine;

public class CampfireInteraction : MonoBehaviour
{
    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && TurnSystem.instance != null)
        {
            // Informer le système de tours que le joueur est près d'un feu de camp
            TurnSystem.instance.SetPlayerNearCampfire(true);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            
            // Informer le système de tours que le joueur a quitté le feu de camp
            if (TurnSystem.instance != null)
            {
                TurnSystem.instance.SetPlayerNearCampfire(false);
            }
        }
    }
}