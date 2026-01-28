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

        int baseAmount = RFIDManager.instance.HasAxe() ? 1 : 1;

        if (EventSystem.instance != null)
        {
            baseAmount *= EventSystem.instance.GetWoodMultiplier();
        }

        Inventory.instance.AddWood(baseAmount);

        if (EventSystem.instance != null && EventSystem.instance.doubleWoodEvent)
        {
            Debug.Log($"Bois récolté : {baseAmount}");
        }
        else
        {
            Debug.Log($"Bois récolté : {baseAmount}");
        }

        Destroy(gameObject);
    }
}