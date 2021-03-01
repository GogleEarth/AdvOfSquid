using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Image fade_image;
    [SerializeField]
    float fade_speed;

    bool fadeout;
    bool gamestart;
    bool fadein;

    void Start()
    {
        fadeout = false;
        gamestart = false;
        fadein = false;
    }

    void Update()
    {
        if (fadeout)
        {
            StartCoroutine("FadeOut");
            fadeout = false;
        }

        if (gamestart)
        {
            SceneManager.LoadScene("LoadScene");
        }

        if (fadein)
        {
            StartCoroutine("FadeIn");
            fadein = false;
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
        gamestart = true;
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

    public void GameStart()
    {
        fadeout = true;
        fade_image.gameObject.SetActive(true);
    }
}
