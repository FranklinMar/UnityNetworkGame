using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;

    private void Start()
    {
        // Synchronyze the scene
        PhotonNetwork.automaticallySyncScene = true;
        // Instantiate player object, spawn it by coordinates, and assign this object to current user
        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(transform.position.x, transform.position.y), Quaternion.identity, 0);
        // Activate scene main camera
        SceneCamera.SetActive(true);
    }
}
