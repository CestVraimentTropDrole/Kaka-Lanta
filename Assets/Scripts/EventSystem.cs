using UnityEngine;
using TMPro;
using System.Collections;

public class EventSystem : MonoBehaviour
{
    public static EventSystem instance;

    [Header("Configuration")]
    [SerializeField] private int minDaysBetweenEvents = 2;
    [SerializeField] private int maxDaysBetweenEvents = 3;
    
    [Header("UI")]
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private TMP_Text eventTitleText;
    [SerializeField] private TMP_Text eventDescriptionText;
    [SerializeField] private float eventDisplayDuration = 5f;
    
    // États des événements actifs
    [HideInInspector] public bool doubleWoodEvent = false;
    [HideInInspector] public bool tempeteEvent = false;
    
    private int daysUntilNextEvent;
    private int currentEventDuration = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (eventPanel != null)
            eventPanel.SetActive(false);
            
        // Premier événement dans 2-3 tours
        daysUntilNextEvent = Random.Range(minDaysBetweenEvents, maxDaysBetweenEvents + 1);
    }

    public void OnTurnStart()
    {
        // Réduire la durée des événements
        if (currentEventDuration > 0)
        {
            currentEventDuration--;
            if (currentEventDuration == 0)
            {
                EndCurrentEvent();
            }
        }
        
        // Vérifier si c'est le moment d'un nouvel événement
        daysUntilNextEvent--;
        if (daysUntilNextEvent <= 0)
        {
            TriggerRandomEvent();
            daysUntilNextEvent = Random.Range(minDaysBetweenEvents, maxDaysBetweenEvents + 1);
        }
    }

    private void TriggerRandomEvent()
    {
        // Annuler l'événement précédent
        EndCurrentEvent();
        
        // Choisir un événement aléatoire
        int randomEvent = Random.Range(0, 2);
        
        switch (randomEvent)
        {
            case 0:
                DoubleWoodEvent();
                break;
        }
    }

    // ========== ÉVÉNEMENTS ==========
    
    private void DoubleWoodEvent()
    {
        doubleWoodEvent = true;
        currentEventDuration = 2;
        ShowEvent("Forêt Généreuse", "Le bois récolté est doublé pendant 2 tours !", Color.green);
        Debug.Log("Événement : Bois x2");
    }

    public void TempeteEvent()
    {
        tempeteEvent = true;
        currentEventDuration = 1;
        ShowEvent("Tempête", "...", Color.green);
    }

    // ========== GESTION DES ÉVÉNEMENTS ==========

    private void EndCurrentEvent()
    {
        doubleWoodEvent = false;
        tempeteEvent = false;
    }

    private void ShowEvent(string title, string description, Color color)
    {
        if (eventPanel == null) return;
        
        if (eventTitleText != null)
        {
            eventTitleText.text = title;
            eventTitleText.color = color;
        }
        
        if (eventDescriptionText != null)
        {
            eventDescriptionText.text = description;
        }
        
        eventPanel.SetActive(true);
        
        // Cacher après quelques secondes
        StartCoroutine(HideEventAfterDelay());
    }

    private IEnumerator HideEventAfterDelay()
    {
        yield return new WaitForSeconds(eventDisplayDuration);
        
        if (eventPanel != null)
            eventPanel.SetActive(false);
    }

    // ========== GETTERS POUR LES AUTRES SCRIPTS ==========
    
    public int GetWoodMultiplier()
    {
        return doubleWoodEvent ? 2 : 1;
    }
}