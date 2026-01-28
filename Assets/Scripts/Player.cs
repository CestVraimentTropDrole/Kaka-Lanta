using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    public int playerNumber = 1; // 1, 2, 3 ou 4
    private string horizontalAxis;
    private string verticalAxis;

    public Animator animator;

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
        animator.SetFloat("RightSpeed", moveInput.x);
        animator.SetFloat("UpSpeed", moveInput.y);

        int direction = 0;
        if (moveInput.x > 0) direction = 1;       // Droite
        else if (moveInput.x < 0) direction = 3;  // Gauche
        else if (moveInput.y > 0) direction = 2;  // Haut
        else if (moveInput.y < 0) direction = 4;  // Bas

        animator.SetInteger("Direction", direction);

        Debug.Log("Mouvement - Player " + playerNumber + "/ MoveInput: " + moveInput);

        // Déplacer le personnage
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}