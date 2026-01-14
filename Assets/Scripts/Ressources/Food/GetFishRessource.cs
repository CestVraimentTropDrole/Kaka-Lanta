using UnityEngine;

public class FishResource : MonoBehaviour
{
    private bool playerInZone = false;
    private PlayerHunger playerHunger; 

    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            HarvestFish();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;

            playerHunger = collision.GetComponent<PlayerHunger>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            playerHunger = null;
        }
    }

    private void HarvestFish()
    {
        if (playerHunger == null)
        {
            return;
        }
        
        if (RFIDManager.instance.HasFishingRod())
        {
            playerHunger.GainHungerFromFood(2);
        }
        else
        {
            return;
        }


        Destroy(gameObject);
    }
}