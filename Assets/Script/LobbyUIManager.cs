using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviour
{
    public GameManager game_manager;
    public Image fade_image;
    public float fade_speed;


    void Start()
    {
        fade_image.gameObject.SetActive(true);

        StartCoroutine("FadeIn");
    }

    void Update()
    {

    }

    IEnumerator FadeOut()
    {
        for (float i = 0f; i <= 1; i += fade_speed * Time.deltaTime)
        {
            Color color = new Vector4(0.0f, 0.0f, 0.0f, i);
            fade_image.color = color;
            yield return 0;
        }
    }

    IEnumerator FadeIn()
    {
        for (float i = 1f; i >= 0; i -= fade_speed * Time.deltaTime)
        {
            Color color = new Vector4(0.0f, 0.0f, 0.0f, i);
            fade_image.color = color;
            yield return 0;
        }
    }

}
