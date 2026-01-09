using UnityEngine;
using TMPro;

public class BuildingGhost : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int woodRequired = 10;
    [SerializeField] private GameObject finalBuilding; // Le bâtiment final à activer
    
    [Header("UI")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private SpriteRenderer Wood;
    
    [Header("Visuel")]
    [SerializeField] private Color canBuildColor = new Color(0, 1, 0, 0.5f); // Vert transparent
    [SerializeField] private Color cannotBuildColor = new Color(1, 0, 0, 0.5f); // Rouge transparent
    
    private SpriteRenderer[] spriteRenderers;
    private bool playerInZone = false;
    private bool isBuilt = false;

    void Start()
    {
        // Récupérer tous les SpriteRenderer pour les rendre transparents
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        // Rendre le fantôme semi-transparent
        SetGhostTransparency();
        
        // Cacher l'UI au départ
        if (uiPanel != null)
            uiPanel.SetActive(false);
            
        // Désactiver le bâtiment final au départ
        if (finalBuilding != null)
            finalBuilding.SetActive(false);
            
        // Mettre à jour le texte du coût
        if (costText != null)
            costText.text = $"Coût : {woodRequired}";
    }

    void Update()
    {
        if (isBuilt || !playerInZone) return;

        // Vérifier si on a assez de bois
        bool canBuild = Inventory.instance != null && 
                       Inventory.instance.GetWoodCount() >= woodRequired;

        // Mettre à jour la couleur du fantôme
        UpdateGhostColor(canBuild);
        
        // Mettre à jour le texte de statut
        if (statusText != null)
        {
            if (canBuild)
                statusText.text = "[E] pour construire";
            else
                statusText.text = " ";
        }

        // Construire si le joueur appuie sur E et a assez de bois
        if (canBuild && Input.GetKeyDown(KeyCode.E))
        {
            Build();
        }
        
        // Alternative: construction avec le bouton RFID
        if (canBuild && RFIDManager.instance != null && RFIDManager.instance.IsButtonPressed())
        {
            Build();
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
        
        Debug.Log($"🏠 Construction terminée ! -{woodRequired} bois");
        
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
                
            Debug.Log("🏗️ Zone de construction");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            
            if (uiPanel != null)
                uiPanel.SetActive(false);
                
            Debug.Log("👋 Sortie de la zone de construction");
        }
    }
}