using UnityEngine;
using System.IO.Ports;
using System;

public class RFIDManager : MonoBehaviour
{
    public static RFIDManager instance;

    [SerializeField] private string portName_m1 = "COM8";
    [SerializeField] private string portName_m2 = "COM10";
    [SerializeField] private string portName_m3 = "COM6";
    [SerializeField] private string portName_m4 = "COM11";
    [SerializeField] private int baudRate = 9600;

    private SerialPort serial1;
    private SerialPort serial2;
    private SerialPort serial3;
    private SerialPort serial4;

    private string lastRFID = "";
    private bool buttonJustPressed = false;

    private bool _AL, _AR, _AU, _AD, _C;

    public bool AL() => _AL;
    public bool AR() => _AR;
    public bool AU() => _AU;
    public bool AD() => _AD;
    public bool C() => _C;

    private SerialPort GetActiveSerial()
    {
        PlayersManager pm = FindFirstObjectByType<PlayersManager>();
        if (pm == null) return null;

        switch (pm.currentPlayer)
        {
            case 0: return serial1;
            case 1: return serial2;
            case 2: return serial3;
            case 3: return serial4;
            default: return null;
        }
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    void Start()
    {
        try
        {
            serial1 = new SerialPort(portName_m1, baudRate);
            serial1.ReadTimeout = 100;
            serial1.Open();
            Debug.Log("Arduino 1 connecté");

            serial2 = new SerialPort(portName_m2, baudRate);
            serial2.ReadTimeout = 100;
            serial2.Open();
            Debug.Log("Arduino 2 connecté");

            serial3 = new SerialPort(portName_m3, baudRate);
            serial3.ReadTimeout = 100;
            serial3.Open();
            Debug.Log("Arduino 3 connecté");

            serial4 = new SerialPort(portName_m4, baudRate);
            serial4.ReadTimeout = 100;
            serial4.Open();
            Debug.Log("Arduino 3 connecté");
        }
        catch (Exception e)
        {
            
        }
    }

    void Update()
    {
        buttonJustPressed = false;
        _AL = _AR = _AU = _AD = _C = false; // Mettre en commentaire pour jouer avec les joysticks

        SerialPort activeSerial = GetActiveSerial();

        if (activeSerial != null && activeSerial.IsOpen && activeSerial.BytesToRead > 0)
        {
            try
            {
                string data = activeSerial.ReadLine().Trim();
                Debug.Log(data);

                // ---------- RFID ----------
                if (data.StartsWith("RFID:"))
                {
                    lastRFID = data;
                }

                // ---------- BOUTON ----------
                else if (data == "BTN:PRESSED")
                {
                    buttonJustPressed = true;
                }

                // ---------- JOYSTICK ----------
                else if (data.StartsWith("JOY:"))
                {
                    _AL = _AR = _AU = _AD = _C = false;

                    switch (data)
                    {
                        case "JOY:LEFT":  _AR = true; break;
                        case "JOY:RIGHT": _AL = true; break;
                        case "JOY:UP":    _AU = true; break;
                        case "JOY:DOWN":  _AD = true; break;
                        case "JOY:CENTER": _C = true; break;
                    }
                }
            }
            catch { }
        }

        // Contrôles clavier
        if (Input.GetKey(KeyCode.Q)) _AL = true;
        if (Input.GetKey(KeyCode.D)) _AR = true;
        if (Input.GetKey(KeyCode.Z)) _AU = true;
        if (Input.GetKey(KeyCode.S)) _AD = true;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonJustPressed = true;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            lastRFID = "RFID:O1";
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            lastRFID = "RFID:O2";
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            lastRFID = "RFID:O3";
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            lastRFID = "RFID:O4";
        }
    }
    
    public bool IsButtonPressed() => buttonJustPressed;

    public bool HasAxe()        => lastRFID == "RFID:O1";
    public bool HasPioche()     => lastRFID == "RFID:O2";
    public bool HasFishingRod() => lastRFID == "RFID:O3";
    public bool HasHeart()      => lastRFID == "RFID:O4";

    public string GetLastRFID() => lastRFID;

    void OnApplicationQuit()
    {
        if (serial1 != null && serial1.IsOpen)
        {
            serial1.Close();
        }

        if (serial2 != null && serial2.IsOpen)
        {
            serial2.Close();
        }

        if (serial3 != null && serial3.IsOpen)
        {
            serial3.Close();
        }

        if (serial4 != null && serial4.IsOpen)
        {
            serial4.Close();
        }
    }
}
