using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager game_manager;
    public Image fade_image;
    public float fade_speed;

    bool fadeout;

    void Start()
    {
        fadeout = false;
    }

    void Update()
    {
        if (fadeout)
        {
            StartCoroutine("FadeOut");
            fadeout = false;
        }
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

    public void GameStart()
    {
        fadeout = true;
        game_manager.SetGameStart(true);
        fade_image.gameObject.SetActive(true);
    }
}
