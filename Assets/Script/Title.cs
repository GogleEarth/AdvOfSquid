using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject title_background1;
    public GameObject title_background2;
    public GameObject start;
    public GameObject exit;

    public float speed;

    Vector2 origin;
    bool title_animation_comp;
    // Start is called before the first frame update
    void Start()
    {
        origin.x = 0.0f;
        origin.y = 0.0f;
        title_animation_comp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!title_animation_comp)
        {
            Vector2 position = title_background1.transform.position;

            if (position.y >= 0.1f)
            {
                Debug.Log("title1의 y값 : " + position.y);

                Vector2 new_position = Vector2.Lerp(position, origin, speed * Time.deltaTime);
                title_background1.transform.position = new_position;
            }
            else
            {
                Color start_color = start.GetComponent<Image>().color;
                Debug.Log("start의 alpha값 : " + start_color.a);
                if (start_color.a < 0.9f)
                {
                    Color new_color = Color.Lerp(start_color, Color.white, speed * Time.deltaTime);
                    start.GetComponent<Image>().color = new_color;
                }

                Color exit_color = exit.GetComponent<Image>().color;
                Debug.Log("exit의 alpha값 : " + exit_color.a);
                if (exit_color.a < 0.9f)
                {
                    Color new_color = Color.Lerp(exit_color, Color.white, speed * Time.deltaTime);
                    exit.GetComponent<Image>().color = new_color;
                }
                else
                {
                    title_animation_comp = true;
                }
            }
        }
    }
}
