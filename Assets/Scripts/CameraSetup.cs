using ShareefSoftware;
using UnityEngine;

public class MazeCameraSetup : MonoBehaviour
{
    [SerializeField] private Camera mazeCamera;  // Reference to the camera

    void Start()
    { 

        // Set the camera position to fixed coordinates (4, 1, -4)
        Vector3 fixedPosition = new Vector3(4f, 1f, -4f);
        mazeCamera.transform.position = fixedPosition;

        // Optionally, rotate the camera to look towards the maze
        mazeCamera.transform.rotation = Quaternion.Euler(90, 180, 0);  // Top-down view facing south
    }
}
