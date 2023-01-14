using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Constant distance of camera to the scene
    [SerializeField] private float z = -10f;
    // Position of player object
    private Vector3 position;
    // Player object
    [SerializeField] public Transform player;

    // Update is called once per frame
    void Update()
    {
        // If player is assigned
        if (player)
        {
            // Get position of player object
            position = player.position;
            position.z = z;

            // Move camera smoothly
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
        }
    }
}
