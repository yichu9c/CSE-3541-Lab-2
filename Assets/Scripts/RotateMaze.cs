using ShareefSoftware;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateMaze : MonoBehaviour
{
    public GameObject createdMaze;
    public float rotationSpeed = 100f; // Speed of rotation
    private bool isRotating = false; // Toggle for rotation
    private Controls controls;


    public LevelGeneration levelGeneration;
    private Vector3 mazeCenter; // Store the calculated center of the maze

    private void Awake()
    {
        levelGeneration = GetComponent<LevelGeneration>();
        controls = new Controls();
        controls.MazeControls.Enable(); // Enable the action map
        controls.MazeControls.ToggleRotation.performed += OnToggleRotation; // Subscribe to the action

        // Calculate the maze center once at the beginning
        mazeCenter = CalculateCenter();
    }

    private void OnDisable()
    {
        controls.MazeControls.ToggleRotation.performed -= OnToggleRotation; // Unsubscribe to avoid memory leaks
        controls.Disable(); // Disable the action map
    }

    private void OnToggleRotation(InputAction.CallbackContext context)
    {
        isRotating = !isRotating; // Toggle the rotation state
    }

    private void Update()
    {
        if (isRotating && createdMaze != null)
        {
            // Rotate around the center of the maze
            createdMaze.transform.RotateAround(mazeCenter, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private Vector3 CalculateCenter()
    {
        float centerX = levelGeneration.GetNumberOfColumns() * levelGeneration.GetCellWidth() / 2f;
        float centerZ = levelGeneration.GetNumberOfRows() * levelGeneration.GetCellHeight() / 2f;
        return new Vector3(centerX, 0, centerZ);
    }
}
