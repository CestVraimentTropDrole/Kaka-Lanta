using UnityEngine;

public class WoodResource : MonoBehaviour
{
    private bool playerInZone = false;

    [Header("Sound")]
    public AudioClip pickupSound;
    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            HarvestWood();
            if (pickupSound != null)
{
    GameObject tempAudio = new GameObject("TempAudio");
    tempAudio.transform.position = transform.position;
    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
    audioSource.clip = pickupSound;
    audioSource.volume = 2f; // Définis le volume AVANT de jouer
    audioSource.Play();
    Destroy(tempAudio, pickupSound.length);
}
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