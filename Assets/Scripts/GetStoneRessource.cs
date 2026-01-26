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

        // BONUS si pioche (objet 1)
        if (RFIDManager.instance.HasPioche())
        {
            Inventory.instance.AddStone(2);
            Debug.Log("🪵 Pierre +2 (pioche) depuis " + gameObject.name);
        }
        else
        {
            Inventory.instance.AddStone(1);
            Debug.Log("🪵 Pierre +1 depuis " + gameObject.name);
        }

        // Détruire l'objet bois après récolte
        Destroy(gameObject);
    }
}