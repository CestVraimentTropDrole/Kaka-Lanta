using UnityEngine;

public class WoodResource : MonoBehaviour
{
    private bool playerInZone = false;

    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            HarvestWood();
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
        }
    }

    private void HarvestWood()
    {
        if (Inventory.instance == null)
        {
            return;
        }

        if (RFIDManager.instance.HasAxe())
        {
            Inventory.instance.AddWood(2);
        }
        else
        {
            Inventory.instance.AddWood(1);
        }

        Destroy(gameObject);
    }
}