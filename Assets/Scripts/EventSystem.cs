using UnityEngine;
using TMPro;
using System.Collections;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem instance;

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
    [HideInInspector] public bool doubleStoneEvent = false;
    [HideInInspector] public bool shortDayEvent = false;
    [HideInInspector] public bool tempeteEvent = false;
    
    private int daysUntilNextEvent;
    private int currentEventDuration = 0;
    private int currentTurnInDay = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }
        if (daysUntilNextEvent == 0)
        {
            // Premier événement dans 2-3 tours
            daysUntilNextEvent = Random.Range(minDaysBetweenEvents, maxDaysBetweenEvents + 1);
            currentTurnInDay = 1;
        }   
    }

    public void OnTurnStart()
    {   
        if (TurnSystem.instance == null)
        {
            return;
        }
        currentTurnInDay++;

        if (currentTurnInDay > TurnSystem.instance.totalPlayers)
        {
            currentTurnInDay = 1;
            OnDayStart();
        }

        // Réduire la durée des événements
        if (currentEventDuration > 0)
        {
            currentEventDuration--;
            if (currentEventDuration == 0)
            {
                EndCurrentEvent();
            }
        }
    }

    private void OnDayStart()
    {
        currentTurnInDay = 1;
        
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
        int randomEvent = Random.Range(0, 3);
        
        switch (randomEvent)
        {
            case 0:
                DoubleWoodEvent();
                break;
            
            case 1:
                DoubleStoneEvent();
                break;
            
            case 2:
                ShortDay();
                break;
        }
    }

    // ========== ÉVÉNEMENTS ==========
    
    private void DoubleWoodEvent()
    {
        doubleWoodEvent = true;
        currentEventDuration = TurnSystem.instance.totalPlayers;
        ShowEvent("Forêt Généreuse", "Le bois récolté est doublé aujourd'hui !", Color.green);
        Debug.Log("Événement : Bois x2");
    }

    private void DoubleStoneEvent()
    {
        doubleStoneEvent = true;
        currentEventDuration = TurnSystem.instance.totalPlayers;
        ShowEvent("Mine Généreuse", "La pierre récoltée est doublé aujourd'hui !", Color.green);
        Debug.Log("Événement : Pierre x2");
    }

    public void ShortDay()
    {
        shortDayEvent = true;
        currentEventDuration = TurnSystem.instance.totalPlayers;

        if (TurnSystem.instance != null)
        {
            TurnSystem.instance.SetShortDay(true);
        }

        ShowEvent("On a gagné du temps..", "Le jour est plus court, personne ne sait pourquoi..", Color.green);
        Debug.Log("Événement : Short Day");
    }

    public void TempeteEvent()
    {
        tempeteEvent = true;
        currentEventDuration = 1;
        ShowEvent("Tempête", "Dernier jour pour construire radeau !", Color.green);
    }

    // ========== GESTION DES ÉVÉNEMENTS ==========

    private void EndCurrentEvent()
    {
        doubleWoodEvent = false;
        doubleStoneEvent = false;
        shortDayEvent = false;
        tempeteEvent = false;

        if (TurnSystem.instance != null)
        {
            TurnSystem.instance.SetShortDay(false);
        }
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
        {
            eventPanel.SetActive(false);
        }
    }

    // ========== GETTERS POUR LES AUTRES SCRIPTS ==========
    
    public int GetWoodMultiplier()
    {
        return doubleWoodEvent ? 2 : 1;
    }

    public int GetStoneMultiplier()
    {
        return doubleStoneEvent ? 2 : 1;
    }

    public bool IsShortDay()
    {
        return shortDayEvent;
    }
}