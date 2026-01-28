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
        moveInput.x = Input.GetAxisRaw(horizontalAxis);
        moveInput.y = Input.GetAxisRaw(verticalAxis);
    }

    void FixedUpdate()
    {
        // Déplacer le personnage
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}