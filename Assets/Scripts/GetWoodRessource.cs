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
            Debug.Log("→ Joueur près du bois: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("← Joueur éloigné du bois: " + gameObject.name);
        }
    }

    private void HarvestWood()
    {
        if (Inventory.instance == null)
        {
            Debug.LogError("Inventory.instance est null!");
            return;
        }

        // BONUS si hache (objet 1)
        if (RFIDManager.instance.HasAxe())
        {
            Inventory.instance.AddWood(2);
            Debug.Log("🪵 Bois +2 (hache) depuis " + gameObject.name);
        }
        else
        {
            Inventory.instance.AddWood(1);
            Debug.Log("🪵 Bois +1 depuis " + gameObject.name);
        }

        // Détruire l'objet bois après récolte
        Destroy(gameObject);
    }
}