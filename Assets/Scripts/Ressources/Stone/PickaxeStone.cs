using UnityEngine;
using UnityEngine.UI;

public class StoneMinigame : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private int minStoneDropped = 2;
    [SerializeField] private int maxStoneDropped = 4;
    [SerializeField] private float dropRadius = 1f;
    
    [Header("Action")]
    [SerializeField] private int successfulHitsRequired = 3;
    [SerializeField] private float indicatorSpeed = 1f;
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private Slider timingBar;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private Image successZoneImage;
    [SerializeField] private float successZoneSize = 0.2f;
    
    private bool playerInZone = false;
    private bool minigameActive = false;
    private float currentIndicatorPosition = 0f;
    private bool movingRight = true;
    private int successfulHits = 0;

    void Start()
    {
        if (minigameUI != null)
            minigameUI.SetActive(false);
            
        // Zone de succès
        if (successZoneImage != null)
        {
            RectTransform rt = successZoneImage.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0.4f, 0f);
                rt.anchorMax = new Vector2(0.6f, 1f);
            }
            successZoneImage.color = new Color(0, 1, 0, 0.3f);
        }
    }

    void Update()
    {
        if (!minigameActive && playerInZone && RFIDManager.instance != null)
        {
            if (RFIDManager.instance.HasPioche() && RFIDManager.instance.IsButtonPressed())
            {
                StartMinigame();
            }
        }

        if (minigameActive)
        {
            UpdateMinigame();
        }
    }

    private void StartMinigame()
    {
        minigameActive = true;
        currentIndicatorPosition = 0f;
        successfulHits = 0;
        movingRight = true;
        
        if (minigameUI != null)
            minigameUI.SetActive(true);
            
        if (timingBar != null)
        {
            timingBar.value = 0f;
        }
    }

    private void UpdateMinigame()
    {
        // Déplacer l'indicateur de gauche à droite
        if (movingRight)
        {
            currentIndicatorPosition += indicatorSpeed * Time.deltaTime;
            if (currentIndicatorPosition >= 1f)
            {
                currentIndicatorPosition = 1f;
                movingRight = false;
            }
        }
        else
        {
            currentIndicatorPosition -= indicatorSpeed * Time.deltaTime;
            if (currentIndicatorPosition <= 0f)
            {
                currentIndicatorPosition = 0f;
                movingRight = true;
            }
        }

        // Mettre à jour la position visuelle
        if (timingBar != null)
        {
            timingBar.value = currentIndicatorPosition;
        }

        if (RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            CheckTiming();
        }

        if (!playerInZone)
        {
            FailMinigame();
        }
    }

    private void CheckTiming()
    {
        float successMin = 0.5f - (successZoneSize / 2f);
        float successMax = 0.5f + (successZoneSize / 2f);
        
        if (currentIndicatorPosition >= successMin && currentIndicatorPosition <= successMax)
        {
            successfulHits++;
            
            if (successZoneImage != null)
            {
                successZoneImage.color = Color.green;
                Invoke(nameof(ResetZoneColor), 0.2f);
            }
            
            if (successfulHits >= successfulHitsRequired)
            {
                SuccessMinigame();
            }
        }
        else
        {   
            // Effet visuel d'échec
            if (successZoneImage != null)
            {
                successZoneImage.color = Color.red;
                Invoke(nameof(ResetZoneColor), 0.2f);
            }

        }
    }

    private void ResetZoneColor()
    {
        if (successZoneImage != null)
        {
            successZoneImage.color = new Color(0, 1, 0, 0.3f);
        }
    }

    private void SuccessMinigame()
    {
        minigameActive = false;
        
        if (minigameUI != null)
            minigameUI.SetActive(false);

        BreakStone();
    }

    private void FailMinigame()
    {
        minigameActive = false;
        
        if (minigameUI != null)
            minigameUI.SetActive(false);
    }

    private void BreakStone()
    {
        if (stonePrefab == null)
        {
            return;
        }

        int stoneAmount = Random.Range(minStoneDropped, maxStoneDropped + 1);

        for (int i = 0; i < stoneAmount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            Instantiate(stonePrefab, spawnPosition, Quaternion.identity);
        }

        Destroy(gameObject);
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
}