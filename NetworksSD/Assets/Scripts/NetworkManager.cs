using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;

    private void Start()
    {
        //GameCanvas.SetActive(true);
        float random = Random.Range(-1f, 1f);
        //PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector2(transform.position.x/* * random*/, transform.position.y), Quaternion.identity, 0);
        SceneCamera.SetActive(true);
    }

    public void SpawnPlayer()
    {
        //GameCanvas.SetActive(false);
        //SceneCamera.SetActive(false);
    }
}
