using UnityEngine;
using UnityEngine.UI;

public class TreeMinigame : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private int minWoodDropped = 2;
    [SerializeField] private int maxWoodDropped = 4;
    [SerializeField] private float dropRadius = 1f;
    
    [Header("Action")]
    [SerializeField] private int clicksRequired = 10;
    [SerializeField] private float decaySpeed = 2f;
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Image fillImage;
    
    private bool playerInZone = false;
    private bool minigameActive = false;
    private float currentProgress = 0f;
    private int currentClicks = 0;

    [Header("Sound")]
    public AudioClip ChopSound;
    void Start()
    {
        if (minigameUI != null)
            minigameUI.SetActive(false);
    }

    void Update()
    {
        if (!minigameActive && playerInZone && RFIDManager.instance != null)
        {
            if (RFIDManager.instance.HasAxe() && RFIDManager.instance.IsButtonPressed())
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
        currentProgress = 0f;
        currentClicks = 0;
        
        if (minigameUI != null)
            minigameUI.SetActive(true);
            
        if (progressBar != null)
        {
            progressBar.value = 0f;
            progressBar.maxValue = clicksRequired;
        }
    }

    private void UpdateMinigame()
    {
        if (RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            currentClicks++;
            currentProgress = currentClicks;
            if (ChopSound != null)
{
    GameObject tempAudio = new GameObject("TempAudio");
    tempAudio.transform.position = transform.position;
    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
    audioSource.clip = ChopSound;
    audioSource.volume = 2f; // Définis le volume AVANT de jouer
    audioSource.Play();
    Destroy(tempAudio, ChopSound.length);
}
        }

        currentProgress -= decaySpeed * Time.deltaTime;
        currentProgress = Mathf.Max(0, currentProgress);

        if (progressBar != null)
        {
            progressBar.value = currentProgress;
            
            if (fillImage != null)
            {
                float ratio = currentProgress / clicksRequired;
                if (ratio >= 0.8f)
                    fillImage.color = Color.green;
                else if (ratio >= 0.5f)
                    fillImage.color = Color.yellow;
                else
                    fillImage.color = Color.red;
            }
        }

        if (currentClicks >= clicksRequired)
        {
            SuccessMinigame();

        }

        if (!playerInZone)
        {
            FailMinigame();
    }
    }   
    private void SuccessMinigame()
    {
        minigameActive = false;
        
        if (minigameUI != null)
            minigameUI.SetActive(false);

        BreakTree();
        
    }

    private void FailMinigame()
    {
        minigameActive = false;
        
        if (minigameUI != null)
            minigameUI.SetActive(false);
    }

    private void BreakTree()
    {
        if (woodPrefab == null)
        {
            return;
        }

        int woodAmount = Random.Range(minWoodDropped, maxWoodDropped + 1);

        for (int i = 0; i < woodAmount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * dropRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
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