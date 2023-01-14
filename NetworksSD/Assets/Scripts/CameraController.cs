using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float z = -10f;
    private Vector3 position;
    [SerializeField] public Transform player;

    // Start is called before the first frame update
    void Start()
    { 
      if (!player)
        {
            //player = FindObjectOfType<Player>().transform;
            return;
        }  
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            position = player.position;
            position.z = z;

            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
        }
    }
}
