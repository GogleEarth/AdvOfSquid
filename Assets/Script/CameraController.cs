using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float camera_lag = 2f;
    public float height = 2f;
    Transform player_transform;

    // Start is called before the first frame update
    void Start()
    {
        player_transform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        Vector2 playerposition = player_transform.position;
        playerposition.y += height;
        Vector3 new_position = Vector2.Lerp(position, playerposition, camera_lag * Time.deltaTime);
        new_position.z = transform.position.z;
        transform.position = new_position;
    }
}
