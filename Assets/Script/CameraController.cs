using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float zoom_speed = 2f;

    float dist;
    Vector3 mouse_start;

    // Start is called before the first frame update
    void Start()
    {
        dist = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Scroll();
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoom_speed;
        if (distance != 0)
        {
            if (Camera.main.orthographicSize > 1 && Camera.main.orthographicSize < 16)
            Camera.main.orthographicSize += distance;
        }
    }

    private void Scroll()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouse_start = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            mouse_start = Camera.main.ScreenToWorldPoint(mouse_start);
            mouse_start.z = transform.position.z;

        }
        else if (Input.GetMouseButton(1))
        {
            var mouse_move = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            mouse_move = Camera.main.ScreenToWorldPoint(mouse_move);
            mouse_move.z = transform.position.z;
            transform.position = transform.position - (mouse_move - mouse_start);
        }
    }
}
