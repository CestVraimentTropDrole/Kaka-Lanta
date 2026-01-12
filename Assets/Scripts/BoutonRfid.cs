using UnityEngine;
using System.IO.Ports;
using System;

public class RFIDManager : MonoBehaviour
{
    public static RFIDManager instance; 

    [Header("Arduino RFID")]
    [SerializeField] private string portRFID = "COM6";

    [Header("Arduino Bouton")]
    [SerializeField] private string portBouton = "COM8";

    [SerializeField] private int baudRate = 9600;

    private SerialPort serialRFID;
    private SerialPort serialBouton;

    private string lastRFID = "";
    private bool buttonJustPressed = false;
    
    private bool rfidConnected = false;
    private bool boutonConnected = false;

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
            return;
        }
    }

    void Start()
    {
        // Tentative de connexion RFID
        try
        {
            serialRFID = new SerialPort(portRFID, baudRate);
            serialRFID.ReadTimeout = 100;
            serialRFID.Open();
            rfidConnected = true;
            Debug.Log("✓ RFID connecté sur " + portRFID);
        }
        catch (Exception e)
        {
            
        }

        // Tentative de connexion Bouton
        try
        {
            serialBouton = new SerialPort(portBouton, baudRate);
            serialBouton.ReadTimeout = 100;
            serialBouton.Open();
            boutonConnected = true;
            Debug.Log("✓ Bouton connecté sur " + portBouton);
        }
        catch (Exception e)
        {
    
        }

        if (!rfidConnected || !boutonConnected)
        {
            Debug.LogWarning("⚠ Certains ports ne sont pas connectés. Le système fonctionnera en mode dégradé.");
        }
    }

    void Update()
    {
        buttonJustPressed = false;

        // ---------- RFID ----------
        if (rfidConnected && serialRFID.IsOpen)
        {
            try
            {
                if (serialRFID.BytesToRead > 0)
                {
                    lastRFID = serialRFID.ReadLine().Trim();
                    Debug.Log("RFID: " + lastRFID);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Erreur lecture RFID: " + e.Message);
            }
        }

        // ---------- BOUTON ----------
        if (boutonConnected && serialBouton.IsOpen)
        {
            try
            {
                if (serialBouton.BytesToRead > 0)
                {
                    string data = serialBouton.ReadLine().Trim();
                    Debug.Log("Bouton: " + data);

                    if (data == "BUTTON PRESSED")
                    {
                        buttonJustPressed = true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Erreur lecture Bouton: " + e.Message);
            }
        }

        // TEST CLAVIER (sans arduino)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Test clavier: Espace pressé");
            buttonJustPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            lastRFID = "Objet 2";
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            lastRFID = "Objet 1";
        }
    }

    public bool IsButtonPressed()
    {
        return buttonJustPressed;
    }

    public bool HasAxe()
    {
        return lastRFID.Contains("Objet 1");
    }

    public bool HasPioche()
    {
        return lastRFID.Contains("Objet 2");
    }

    public bool HasHeart()
    {
        return lastRFID.Contains("Objet 4");
    }

    public string GetLastRFID()
    {
        return lastRFID;
    }

    void OnApplicationQuit()
    {
        CloseSerialPorts();
    }

    void OnDestroy()
    {
        CloseSerialPorts();
    }

    void OnDisable()
    {
        CloseSerialPorts();
    }

    private void CloseSerialPorts()
    {
        try
        {
            if (serialRFID != null && serialRFID.IsOpen)
            {
                serialRFID.Close();
                Debug.Log("Port RFID fermé");
            }
        }
        catch { }

        try
        {
            if (serialBouton != null && serialBouton.IsOpen)
            {
                serialBouton.Close();
                Debug.Log("Port Bouton fermé");
            }
        }
        catch { }
    }
}