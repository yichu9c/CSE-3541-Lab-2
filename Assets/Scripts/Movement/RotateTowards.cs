using UnityEngine;
using UnityEngine.InputSystem;

public class RotateTowards : MonoBehaviour
{
    public float sensitivity = 1.0f; // Sensitivity for input
    public float verticalClamp = 80.0f; // Clamp for vertical rotation

    private float rotationX = 0.0f; // Pitch
    private float rotationY = 0.0f; // Yaw

    private Controls playerInputActions;

    private void Awake()
    {
        playerInputActions = new Controls(); 
    }

    private void OnEnable()
    {
        playerInputActions.Player.Look.Enable(); 
    }

    private void OnDisable()
    {
        playerInputActions.Player.Look.Disable(); 
    }

    private void Update()
    {

        Vector2 lookInput = playerInputActions.Player.Look.ReadValue<Vector2>();

        // Adjust yaw and pitch based on input, multiplied by Time.deltaTime for smoothness
        rotationY += lookInput.x * sensitivity * Time.deltaTime; 
        rotationX -= lookInput.y * sensitivity * Time.deltaTime; 

        // Clamp the vertical rotation (pitch) to avoid flipping
        rotationX = Mathf.Clamp(rotationX, -verticalClamp, verticalClamp);

        // Apply yaw rotation to the player (or character) transform
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }
}
