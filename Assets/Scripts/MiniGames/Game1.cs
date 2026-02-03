using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class F1LightsMinigame : MonoBehaviour
{
    [Header("UI - Lumières")]
    [SerializeField] private Image[] redLights; // 5 lumières rouges
    [SerializeField] private TMP_Text instructionText;
    
    [Header("UI - Joueurs")]
    [SerializeField] private Transform playersContainer;
    [SerializeField] private GameObject playerDisplayPrefab; // Prefab pour afficher un joueur
    
    [Header("Configuration")]
    [SerializeField] private float minWaitTime = 2f;
    [SerializeField] private float maxWaitTime = 5f;
    [SerializeField] private float lightOnDelay = 0.5f;
    [SerializeField] private string mainSceneName = "SampleScene";
    
    [Header("Sons")]
    [SerializeField] private AudioClip lightOnSound;
    [SerializeField] private AudioClip lightsOffSound;
    [SerializeField] private AudioClip tooEarlySound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip winnerSound;
    
    private enum GameState { WaitingToStart, LightingUp, WaitingForGo, Racing, Finished }
    private GameState currentState = GameState.WaitingToStart;
    
    private List<PlayerData> players = new List<PlayerData>();
    private bool[] hasReacted;
    private AudioSource audioSource;
    private float raceStartTime;
    
    private class PlayerData
    {
        public int playerIndex; // 0, 1, 2, 3
        public Color playerColor;
        public float reactionTime = -1f; // -1 = pas réagi, -2 = trop tôt
        public GameObject displayObject;
        public Image characterIcon;
        public TMP_Text reactionTimeText;
        public GameObject tooEarlyMarker;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        SetupPlayers();
        ShowInstructions();
        StartCoroutine(StartGameSequence());
    }

    void Update()
    {
        if (currentState == GameState.Racing || currentState == GameState.WaitingForGo)
        {
            CheckPlayerInputs();
        }
    }

    private void SetupPlayers()
    {
        int numberOfPlayers = 2;
        Color[] playerColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
        
        if (GameData.instance != null)
        {
            numberOfPlayers = GameData.instance.numberPlayers;
        }
        
        hasReacted = new bool[numberOfPlayers];
        
        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerData data = new PlayerData
            {
                playerIndex = i,
                playerColor = playerColors[i]
            };
            
            if (playerDisplayPrefab != null && playersContainer != null)
            {
                GameObject display = Instantiate(playerDisplayPrefab, playersContainer);
                data.displayObject = display;
                
                // Récupérer les composants
                data.characterIcon = display.transform.Find("CharacterIcon")?.GetComponent<Image>();
                data.reactionTimeText = display.transform.Find("ReactionTimeText")?.GetComponent<TMP_Text>();
                data.tooEarlyMarker = display.transform.Find("TooEarlyMarker")?.gameObject;
                
                // Appliquer la couleur du joueur
                if (data.characterIcon != null)
                    data.characterIcon.color = data.playerColor;
                
                // Texte initial
                if (data.reactionTimeText != null)
                {
                    data.reactionTimeText.text = $"JOUEUR {i + 1}\nEn attente...";
                    data.reactionTimeText.color = Color.white;
                }
                
                // Cacher le marqueur "trop tôt"
                if (data.tooEarlyMarker != null)
                    data.tooEarlyMarker.SetActive(false);
            }
            
            players.Add(data);
        }
        
        Debug.Log($"🏁 {numberOfPlayers} joueurs configurés");
    }

    private void ShowInstructions()
    {
        if (instructionText != null)
        {
            instructionText.text = "Attendez que les lumières s'éteignent\npuis appuyez sur votre bouton !\n\n" +
                                 "Le plus rapide récupère toute sa VIE !";
            instructionText.fontSize = 32;
            instructionText.color = Color.white;
        }
    }

    private IEnumerator StartGameSequence()
    {
        currentState = GameState.WaitingToStart;
        yield return new WaitForSeconds(3f);
        
        // Allumer les lumières
        currentState = GameState.LightingUp;
        
        if (instructionText != null)
        {
            instructionText.text = "ATTENTION...";
            instructionText.fontSize = 64;
            instructionText.color = Color.yellow;
        }
        
        for (int i = 0; i < redLights.Length; i++)
        {
            if (redLights[i] != null)
            {
                redLights[i].color = Color.red;
                
                if (audioSource != null && lightOnSound != null)
                    audioSource.PlayOneShot(lightOnSound);
            }
            
            yield return new WaitForSeconds(lightOnDelay);
        }
        
        // Attente aléatoire
        currentState = GameState.WaitingForGo;
        float randomWait = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(randomWait);
        
        // GO !
        currentState = GameState.Racing;
        raceStartTime = Time.time;
        
        foreach (Image light in redLights)
        {
            if (light != null)
                light.color = new Color(0.1f, 0.1f, 0.1f); // Presque noir
        }
        
        if (audioSource != null && lightsOffSound != null)
            audioSource.PlayOneShot(lightsOffSound);
        
        if (instructionText != null)
        {
            instructionText.text = "GO !!!";
            instructionText.fontSize = 64;
            instructionText.color = Color.green;
        }
        
        Debug.Log("🚦 GO !");
    }

    private void CheckPlayerInputs()
    {
        // Joueur 1 - Touche 1
        if (!hasReacted[0] && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) || RFIDManager.instance.IsButtonPressed()))
        {
            RegisterReaction(0);
        }
        
        // Joueur 2 - Touche 2
        if (players.Count > 1 && !hasReacted[1] && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)))
        {
            RegisterReaction(1);
        }
        
        // Joueur 3 - Touche 3
        if (players.Count > 2 && !hasReacted[2] && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)))
        {
            RegisterReaction(2);
        }
        
        // Joueur 4 - Touche 4
        if (players.Count > 3 && !hasReacted[3] && (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)))
        {
            RegisterReaction(3);
        }
        
        // Vérifier si tous ont réagi
        if (currentState == GameState.Racing)
        {
            bool allReacted = true;
            foreach (bool reacted in hasReacted)
            {
                if (!reacted) allReacted = false;
            }
            
            if (allReacted)
            {
                EndGame();
            }
        }
    }

    private void RegisterReaction(int playerIndex)
    {
        if (currentState == GameState.WaitingForGo)
        {
            // TROP TÔT !
            players[playerIndex].reactionTime = -2f;
            hasReacted[playerIndex] = true;
            
            if (players[playerIndex].reactionTimeText != null)
            {
                players[playerIndex].reactionTimeText.text = $"JOUEUR {playerIndex + 1}\nTROP TÔT !";
                players[playerIndex].reactionTimeText.color = Color.red;
                players[playerIndex].reactionTimeText.fontSize = 36;
            }
            
            if (players[playerIndex].tooEarlyMarker != null)
                players[playerIndex].tooEarlyMarker.SetActive(true);
            
            if (audioSource != null && tooEarlySound != null)
                audioSource.PlayOneShot(tooEarlySound);
            
            Debug.Log($"Joueur {playerIndex + 1} : TROP TÔT !");
        }
        else if (currentState == GameState.Racing)
        {
            // Temps de réaction valide
            float reactionTime = (Time.time - raceStartTime) * 1000f; // En millisecondes
            players[playerIndex].reactionTime = reactionTime;
            hasReacted[playerIndex] = true;
            
            if (players[playerIndex].reactionTimeText != null)
            {
                players[playerIndex].reactionTimeText.text = $"JOUEUR {playerIndex + 1}\n{reactionTime:F0} ms";
                players[playerIndex].reactionTimeText.color = Color.green;
                players[playerIndex].reactionTimeText.fontSize = 42;
            }
            
            if (audioSource != null && successSound != null)
                audioSource.PlayOneShot(successSound);
            
            Debug.Log($"Joueur {playerIndex + 1} : {reactionTime:F0} ms");
        }
    }

    private void EndGame()
    {
        currentState = GameState.Finished;
        
        // Marquer ceux qui n'ont pas réagi
        for (int i = 0; i < players.Count; i++)
        {
            if (!hasReacted[i])
            {
                if (players[i].reactionTimeText != null)
                {
                    players[i].reactionTimeText.text = $"JOUEUR {i + 1}\nAucune réaction";
                    players[i].reactionTimeText.color = Color.gray;
                }
            }
        }
        
        // Trouver et afficher le gagnant
        int winnerIndex = FindWinner();
        ShowWinner(winnerIndex);
        
        // Sauvegarder le gagnant
        if (winnerIndex >= 0 && GameData.instance != null)
        {
            GameData.instance.minigameWinnerPlayerIndex = winnerIndex;
            Debug.Log($"Joueur {winnerIndex + 1} remporte le mini-jeu !");
        }
        
        // Retour à la scène principale après 5 secondes
        Invoke(nameof(ReturnToMainScene), 5f);
    }

    private int FindWinner()
    {
        int winnerIndex = -1;
        float bestTime = float.MaxValue;
        
        for (int i = 0; i < players.Count; i++)
        {
            // Seulement ceux qui ont un temps valide (pas trop tôt, pas raté)
            if (players[i].reactionTime > 0 && players[i].reactionTime < bestTime)
            {
                bestTime = players[i].reactionTime;
                winnerIndex = i;
            }
        }
        
        return winnerIndex;
    }

    private void ShowWinner(int winnerIndex)
    {
        if (instructionText != null)
        {
            if (winnerIndex >= 0)
            {
                instructionText.text = $"JOUEUR {winnerIndex + 1} GAGNE !\n{players[winnerIndex].reactionTime:F0} ms\n\n\nVie complètement restaurée !";
                instructionText.fontSize = 48;
                instructionText.color = Color.yellow;
                
                // Mettre en évidence le gagnant
                if (players[winnerIndex].reactionTimeText != null)
                {
                    players[winnerIndex].reactionTimeText.text = $"🏆 GAGNANT 🏆\n{players[winnerIndex].reactionTime:F0} ms";
                    players[winnerIndex].reactionTimeText.fontSize = 52;
                }
                
                // Effet visuel sur l'icône du gagnant
                if (players[winnerIndex].characterIcon != null)
                {
                    StartCoroutine(PulseWinnerIcon(players[winnerIndex].characterIcon));
                }
                
                if (audioSource != null && winnerSound != null)
                    audioSource.PlayOneShot(winnerSound);
            }
            else
            {
                instructionText.text = "Aucun gagnant...\nTous disqualifiés ou absents !";
                instructionText.fontSize = 42;
                instructionText.color = Color.red;
            }
        }
    }

    private IEnumerator PulseWinnerIcon(Image icon)
    {
        for (int i = 0; i < 10; i++)
        {
            icon.transform.localScale = Vector3.one * 1.2f;
            yield return new WaitForSeconds(0.2f);
            icon.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ReturnToMainScene()
    {
        Debug.Log("🔄 Retour à la scène principale...");
        Time.timeScale = 1f; // S'assurer que le temps est normal
        SceneManager.LoadScene(mainSceneName);
    }
}