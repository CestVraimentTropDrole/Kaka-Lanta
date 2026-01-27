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
        moveInput = Vector2.zero;

        if (RFIDManager.instance.AL()) moveInput.x = -1;
        if (RFIDManager.instance.AR()) moveInput.x = 1;
        if (RFIDManager.instance.AU()) moveInput.y = 1;
        if (RFIDManager.instance.AD()) moveInput.y = -1;
    }


    void FixedUpdate()
    {
        // Déplacer le personnage
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}