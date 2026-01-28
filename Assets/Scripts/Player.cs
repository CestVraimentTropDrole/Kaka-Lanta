using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public int playerNumber = 1; // 1, 2, 3 ou 4
    private string horizontalAxis;
    private string verticalAxis;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Récupérer les entrées du clavier
        horizontalAxis = "Horizontal" + playerNumber;
        verticalAxis = "Vertical" + playerNumber;
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (RFIDManager.instance.AL()) moveInput.x = -1;
        if (RFIDManager.instance.AR()) moveInput.x = 1;
        if (RFIDManager.instance.AU()) moveInput.y = 1;
        if (RFIDManager.instance.AD()) moveInput.y = -1;
        if (RFIDManager.instance.C()) {
            moveInput.y = 0;
            moveInput.x = 0;
        }
    }


    void FixedUpdate()
    {
        // Déplacer le personnage
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}