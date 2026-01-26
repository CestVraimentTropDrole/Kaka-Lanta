using UnityEngine;

public class HouseInteraction : MonoBehaviour
{
    private bool playerInZone = false;

    void Update()
    {
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed() && TurnSystem.instance != null)
        {
            // Informer le système de tours que le joueur est dans une maison
            TurnSystem.instance.SetPlayerInHouse(true);
            
            // Finir automatiquement le tour quand on rentre dans la maison
            TurnSystem.instance.EndTurn();
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
            
            // Informer le système de tours que le joueur a quitté la maison
            if (TurnSystem.instance != null)
            {
                TurnSystem.instance.SetPlayerInHouse(false);
            }
        }
    }
}