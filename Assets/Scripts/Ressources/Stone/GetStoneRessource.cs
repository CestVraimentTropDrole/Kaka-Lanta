using UnityEngine;

public class StoneResource : MonoBehaviour
{
    private bool playerInZone = false;

    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            HarvestStone();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("→ Joueur près du stone: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("← Joueur éloigné du stone: " + gameObject.name);
        }
    }

    private void HarvestStone()
    {
        if (Inventory.instance == null)
        {
            Debug.LogError("Inventory.instance est null!");
            return;
        }
            Inventory.instance.AddStone(1);

        Destroy(gameObject);
    }
}