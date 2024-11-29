using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    public float floatSpeed = 1f;   
    public float floatHeight = 0.5f; 
    public float rotateSpeed = 50f; 

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Make the coin move up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate the coin continuously
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }
}
