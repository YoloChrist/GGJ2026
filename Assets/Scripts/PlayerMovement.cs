using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private ForceMode2D forceMode = ForceMode2D.Force;
    [SerializeField] private bool canMove = true;
    [SerializeField] private int animationDirection;
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnMoveInput += HandleMoveInput;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnMoveInput -= HandleMoveInput;
    }

    private void HandleMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        if (rb == null) return;

        if (!canMove) return;

        if (moveInput.x > 0)
        {
            animationDirection = 2;
        }
        else if (moveInput.x < 0)
        {
            animationDirection = 4;
        }
        else if (moveInput.y > 0)
        {
            animationDirection = 1;
        }
        else if (moveInput.y < 0)
        {
            animationDirection = 3;
        }
        else if (moveInput.x == 0 && moveInput.y == 0)
        {
            animationDirection = 0;
        }

        anim.SetInteger("direction", animationDirection);

        rb.AddForce(moveInput * moveSpeed, forceMode);
    }

    public void SetcanMoveTrue()
    {
        canMove = true;
    }

    public void SetcanMoveFalse()
    {
        canMove = false;
    }
}