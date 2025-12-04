using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Récupérer les entrées du clavier
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D ou Flèches gauche/droite
        moveInput.y = Input.GetAxisRaw("Vertical");   // W/S ou Flèches haut/bas
    }

    void FixedUpdate()
    {
        // Déplacer le personnage
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}