using UnityEngine;

public class StoneResource : MonoBehaviour
{
    private bool playerInZone = false;

    [Header("Sound")]
    public AudioClip pickupSound;

    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            HarvestStone();
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

    private void HarvestStone()
    {
        if (Inventory.instance == null)
        {
            return;
        }
            Inventory.instance.AddStone(1);

        Destroy(gameObject);
    }
}