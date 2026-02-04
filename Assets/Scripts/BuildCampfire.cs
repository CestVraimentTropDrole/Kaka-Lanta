using UnityEngine;
using TMPro;

public class BuildCampfire : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int woodRequired = 5;

    [SerializeField] private int stoneRequired = 3;
    [SerializeField] private GameObject finalBuilding; // Le bâtiment final à activer
    
    [Header("UI")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TMP_Text costWoodText;

    [SerializeField] private TMP_Text costStoneText;
    
    [Header("Visuel")]
    [SerializeField] private Color canBuildColor = new Color(0, 1, 0, 0.5f); // Vert transparent
    [SerializeField] private Color cannotBuildColor = new Color(1, 0, 0, 0.5f); // Rouge transparent
    
    private SpriteRenderer[] spriteRenderers;
    private bool playerInZone = false;
    private bool isBuilt = false;

    [Header("Sound")]
    public AudioClip BuildSound;

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        SetGhostTransparency();
        
        // Cacher l'UI au départ
        if (uiPanel != null)
            uiPanel.SetActive(false);
            
        // Désactiver le bâtiment final au départ
        if (finalBuilding != null)
            finalBuilding.SetActive(false);

        // Mettre à jour le texte du coût
        if (costWoodText != null)
            costWoodText.text = $"Coût : {woodRequired}";

        if (costStoneText != null)
            costStoneText.text = $"Coût : {stoneRequired}";
    }

    void Update()
    {
        if (isBuilt || !playerInZone) return;

        // Vérifier si on a assez de bois
        bool canBuild = Inventory.instance != null && 
                       Inventory.instance.GetWoodCount() >= woodRequired;

        UpdateGhostColor(canBuild);

        // Construire
        if (canBuild && Input.GetKeyDown(KeyCode.E))
        {
            Build();
if (BuildSound != null)
{
    GameObject tempAudio = new GameObject("TempAudio");
    tempAudio.transform.position = transform.position;
    AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
    audioSource.clip = BuildSound;
    audioSource.volume = 2f; // Définis le volume AVANT de jouer
    audioSource.Play();
    Destroy(tempAudio, BuildSound.length);
}
        }
        
        if (canBuild && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            Build();
            if (BuildSound != null)
    {
        AudioSource.PlayClipAtPoint(BuildSound, transform.position);
    }
        }
    }

    private void SetGhostTransparency()
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            Color color = sr.color;
            color.a = 0.5f; // Semi-transparent
            sr.color = color;
        }
    }

    private void UpdateGhostColor(bool canBuild)
    {
        Color targetColor = canBuild ? canBuildColor : cannotBuildColor;
        
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = targetColor;
        }
    }

    private void Build()
    {
        if (Inventory.instance == null) return;

        // Retirer le bois de l'inventaire
        Inventory.instance.RemoveWood(woodRequired);

        Inventory.instance.RemoveStone(stoneRequired);
        
        
        // Activer le bâtiment final
        if (finalBuilding != null)
        {
            finalBuilding.SetActive(true);
        }
        
        // Cacher l'UI
        if (uiPanel != null)
            uiPanel.SetActive(false);
        
        // Marquer comme construit
        isBuilt = true;
        
        // Désactiver le fantôme
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isBuilt)
        {
            playerInZone = true;
            
            if (uiPanel != null)
                uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            
            if (uiPanel != null)
                uiPanel.SetActive(false);
        }
    }
}