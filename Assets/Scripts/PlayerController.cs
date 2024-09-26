using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotationSpeed = 5f;
    private Animator animator;

    private void Awake()
    {
        playerInput = new Player();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>(); 
        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);
        animator.SetBool("isMoving", move != Vector3.zero);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        if (playerInput.PlayerMain.Jump.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetBool("isJumping", true);
        }
        else if (groundedPlayer)
        {
            animator.SetBool("isJumping", false);
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        Vector2 lookInput = playerInput.PlayerMain.Look.ReadValue<Vector2>();
        RotateCameraAndPlayer(lookInput);
    }
    private void RotateCameraAndPlayer(Vector2 lookInput)
    {
        if (lookInput != Vector2.zero)
        {
            float yaw = lookInput.x * rotationSpeed * Time.deltaTime;
            float pitch = lookInput.y * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, yaw, 0);
            Camera.main.transform.Rotate(-pitch, 0, 0);
        }
    }
}
