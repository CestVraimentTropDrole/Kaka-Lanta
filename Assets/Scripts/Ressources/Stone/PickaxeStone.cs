using UnityEngine;

public class PickaxeStone : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject stonePrefab; // Le prefab de petite pierre à ramasser
    [SerializeField] private int minStoneDropped = 2;
    [SerializeField] private int maxStoneDropped = 4;
    [SerializeField] private float dropRadius = 1f; // Rayon autour du rocher où les pierres tombent
    
    private bool playerInZone = false;

    void Update()
    {
        // Si le joueur est dans la zone ET que le bouton est pressé
        if (playerInZone && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            BreakStone();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("→ Joueur près du rocher: " + gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("← Joueur éloigné du rocher: " + gameObject.name);
        }
    }

    private void BreakStone()
    {
        // Vérifier si le joueur a une pioche
        if (RFIDManager.instance == null || !RFIDManager.instance.HasPioche())
        {
            Debug.Log("⛏️ Vous avez besoin d'une pioche pour casser ce rocher !");
            return;
        }

        // Vérifier que le prefab existe
        if (stonePrefab == null)
        {
            Debug.LogError("Le prefab de pierre n'est pas assigné !");
            return;
        }

        // Déterminer combien de pierres vont tomber
        int stoneAmount = Random.Range(minStoneDropped, maxStoneDropped + 1);
        Debug.Log($"🪨 Rocher cassé ! {stoneAmount} pierres tombent");

        // Faire spawn les pierres autour du rocher
        for (int i = 0; i < stoneAmount; i++)
        {
            // Position aléatoire autour du rocher
            Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            // Créer la pierre
            Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
        }

        // Détruire le gros rocher
        Destroy(gameObject);
    }
}