using UnityEngine;
using UnityEngine.InputSystem;

public class MoveTowards : MonoBehaviour
{
    public float moveSpeed = 3.0f; 
    public Camera playerCamera; 

    private Controls playerInputActions; 
    private CharacterController characterController; 

    private void Awake()
    {
        playerInputActions = new Controls(); 
        characterController = GetComponent<CharacterController>(); 
        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>(); 
        }
    }

    private void OnEnable()
    {
        playerInputActions.Player.Move.Enable(); 
    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.Disable(); 
    }

    private void Update()
    {
        
        Vector2 moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        // Create a movement direction based on the camera's forward and right vectors
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Zero out the y component to avoid moving up/down
        forward.y = 0;
        right.y = 0;

        
        forward.Normalize();
        right.Normalize();

        
        Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

        
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
