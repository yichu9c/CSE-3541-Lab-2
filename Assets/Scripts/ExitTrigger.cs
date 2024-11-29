using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class ExitTrigger : MonoBehaviour
{
    [SerializeField] private Canvas gameOverCanvas; 

    private void Start()
    {
        // Initially hide the Game Over message
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(false);
        }
    }

    // Detect when the camera enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player or camera has entered the exit zone
        if (other.CompareTag("MainCamera"))
        {
            // Display "Game Over"
            if (gameOverCanvas != null)
            {
                gameOverCanvas.gameObject.SetActive(true);
            }

            Debug.Log("Game Over");
            Invoke("QuitGame", 3f);  // Wait for 3 seconds before quitting
        }
    }
    private void QuitGame()
    {

        Application.Quit();

        // If running in the Unity editor, stop the play mode
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}
